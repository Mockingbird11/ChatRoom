using ChatRoom.DataAcces.Log4Net;
using ChatRoom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ChatRoom.Areas.ChatCenter.Models;

namespace ChatRoom.Areas.ChatCenter.Services
{
    public class WebsocketHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        //注入IHttpContextAccessor
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public Logger logger = new Logger();
        public WebsocketHandlerMiddleware(
            RequestDelegate next, IHttpContextAccessor httpContextAccessor
            )
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    //从session中获取当前登录用户Id
                    string clientId = CurrentUser.UserId;
                    var wsClient = new WebsocketClient
                    {
                        Id = clientId,
                        WebSocket = webSocket
                    };
                    try
                    {
                        await Handle(wsClient);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Echo websocket client {clientId} err .", ex);
                        await context.Response.WriteAsync("closed");
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Handle(WebsocketClient webSocket)
        {
            WebsocketClientCollection.Add(webSocket);
            logger.Info($"Websocket client added.");
            WebSocketReceiveResult result = null;
            do
            {
                var buffer = new byte[1024 * 1];
                //等待接收消息
                result = await webSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var msgString = Encoding.UTF8.GetString(buffer);
                    logger.Info($"Websocket client ReceiveAsync message {msgString}.");
                    var message = JsonConvert.DeserializeObject<Message>(msgString);

                    message.SendClientId = message.SendClientId;
                    MessageRoute(message);
                }
            }
            while (!result.CloseStatus.HasValue);
            WebsocketClientCollection.Remove(webSocket);
            logger.Info($"Websocket client closed.");
        }

        private void MessageRoute(Message message)
        {
            var client = WebsocketClientCollection.Get(CurrentUser.UserId);
            switch (message.Action)
            {
                case "join":
                    var joinJson = new
                    {
                        Msg = message.Msg,
                        Sender = message.SendClientId,
                        RoomId = client.RoomNo,
                        MyClientId = "",
                        MessageTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                        NickName = "",
                        Action = message.Action
                    };
                    client.RoomNo = message.Msg;
                    logger.Info($"Websocket client {message.SendClientId} join room {client.RoomNo}.");
                    break;
                case "send_to_room":
                    client.RoomNo = "8888";
                    if (string.IsNullOrEmpty(client.RoomNo))
                    {
                        break;
                    }
                    var clients = WebsocketClientCollection.GetRoomClients(client.RoomNo);
                    var msgJson = new
                    {
                        Msg = message.Msg,
                        Sender = CurrentUser.UserId,
                        RoomId = client.RoomNo,
                        MessageTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                        NickName = CurrentUser.UserName,
                        Action = message.Action
                    };
                    clients.ForEach(c =>
                    {
                        c.SendMessageAsync(JsonConvert.SerializeObject(msgJson));
                    });
                    logger.Info($"Websocket client {message.SendClientId} send message {message.Msg} to room {client.RoomNo}");

                    break;
                case "leave":
                    var roomNo = client.RoomNo;
                    client.RoomNo = "";
                    client.SendMessageAsync($"{message.NickName} leave room {roomNo} success .");
                    logger.Info($"Websocket client {message.SendClientId} leave room {roomNo}");
                    break;
                default:
                    break;
            }
        }
    }
}
