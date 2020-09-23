

//WebSocket相关

var server = 'ws://localhost:54321'; //如果开启了https则这里是wss

var WEB_SOCKET = new WebSocket(server + '/ws');

//获取我的UserId
var MyUserId = localStorage.getItem("UserId");

//连接成功
WEB_SOCKET.onopen = function (evt) {
    console.log('Connection open ...');
    $('#msgList').val('websocket connection opened .');
};

//接受消息
WEB_SOCKET.onmessage = function (evt) {
    console.log('Received Message: ' + evt.data);
    if (evt.data) {
        var data = JSON.parse(evt.data);
        var position = "";
        if (data.Sender == MyUserId) {
            position = "right";
        }

        var param = {
            message: data.Msg,
            messagetime: data.MessageTime,
            myname: data.NickName,
            position: position
        }

        BaseObj.VueApp.addComponent("mymessage", param);
    }
};

//连接关闭
WEB_SOCKET.onclose = function (evt) {
    console.log('Connection closed.');
};

//$('#btnJoin').on('click', function () {
//    var roomNo = $('#txtRoomNo').val();
//    var nick = $('#txtNickName').val();
//    if (roomNo) {
//        var msg = {
//            action: 'join',
//            msg: roomNo,
//            nick: nick
//        };
//        WEB_SOCKET.send(JSON.stringify(msg));
//    }
//});

//$('#btnSend').on('click', function () {
//    var message = $('#txtMsg').val();
//    var nick = $('#txtNickName').val();
//    if (message) {
//        WEB_SOCKET.send(JSON.stringify({
//            action: 'send_to_room',
//            msg: message,
//            nick: nick
//        }));
//    }
//});

//$('#btnLeave').on('click', function () {
//    var nick = $('#txtNickName').val();
//    var msg = {
//        action: 'leave',
//        msg: '',
//        nick: nick
//    };
//    WEB_SOCKET.send(JSON.stringify(msg));
//});
