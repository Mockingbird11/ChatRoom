using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using CharRoom.Entity;
using CharRoom.Entity.CommonModels;
using ChatRoom.Areas.ChatCenter.Models;
using ChatRoom.Controllers;
using ChatRoom.DataAcces.Log4Net;
using ChatRoom.DataAcces.Redis;
using ChatRoom.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using StackExchange.Redis;

namespace ChatRoom.Areas.Login.Controllers
{
    [Area("Users")]
    public class UsersController : BaseController
    {
        private readonly IAppLoginService appLoginService;
        private readonly IDatabase redisDb;
        private readonly RedisHelper redisHelper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UsersController(IAppLoginService _appLoginService, RedisHelper redisHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.appLoginService = _appLoginService;
            this.redisDb = redisHelper.GetDatabase();
            this.redisHelper = redisHelper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult LoginIndex()
        {
            return View();
        }

        public IActionResult RegistIndex()
        {
            return View();
        }

        public async Task<IActionResult> Login(string userName, string passWord)
        {

            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                {
                    resultModel.NormalError("用户名或密码不能为空...");
                }
                else
                {
                    var userInfo = await appLoginService.IsUserExist(userName, passWord);
                    resultModel.Code = userInfo != null ? 1 : -1;
                    resultModel.Msg = userInfo != null ? "成功" : "登陆失败，请核对用户名和密码...";
                    if (resultModel.Code == 1)
                    {
                        resultModel.Data = userInfo;
                        //将用户Id存入Session
                        //httpContextAccessor.HttpContext.Session.SetString("CurrentUserId", userInfo.UserId);
                        CurrentUser.Configure(httpContextAccessor);
                        CurrentUser.SetSession(userInfo);
                        var userList = redisHelper.ListRange<UsersInfo>("OnlineUsers");
                        //若当前用户已存在于在线用户列表中，则不添加进Redis缓存
                        if (!userList.Select(x => x.UserId == userInfo.UserId).FirstOrDefault())
                        {
                            //向列表尾部添加数据，如果当前Key不存在，则先新建一个空Key，再添加数据
                            redisHelper.ListRightPush("OnlineUsers", userInfo);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                resultModel.NormalError(ex.Message);
                logger.Error($"Error—Login/Login/userName:{userName},passWord:{passWord}", ex);
            }
            return Json(resultModel);
        }

        public async Task<IActionResult> Regist(string userName, string passWord)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                {
                    resultModel.NormalError("用户名或密码不能为空...");
                }
                else
                {
                    UsersInfo usersInfo = new UsersInfo()
                    {
                        UserId = Guid.NewGuid().ToString(),
                        UserName = userName,
                        UserPassWord = passWord
                    };
                    var isExist = await appLoginService.IsUserExist(userName);
                    if (isExist)
                    {
                        resultModel.NormalError("该用户名已经存在，再选一个吧...");
                    }
                    else
                    {
                        var isSuccess = await appLoginService.Add(usersInfo);
                        resultModel.Code = isSuccess > 0 ? 1 : -1;
                    }


                }
            }
            catch (Exception ex)
            {
                resultModel.NormalError(ex.Message);
                logger.Error($"Error—Users/Regist/userName:{userName},passWord:{passWord}", ex);
            }
            return Json(resultModel);
        }
    }
}
