using CharRoom.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.Areas.ChatCenter.Models
{
    public static class CurrentUser
    {
        private static IHttpContextAccessor _httpContextAccessor;

        private static ISession _session => _httpContextAccessor.HttpContext.Session;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///用户编号 
        /// </summary>
        public static string UserId
        {
            get => _session == null ? "" : _session.GetString("UserId");
            set => _session.SetString("UserId", value != "" ? value.ToString() : "0");
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string UserName
        {
            get => _session == null ? "" : _session.GetString("UserName");
            set => _session.SetString("UserName", !string.IsNullOrEmpty(value) ? value : "");
        }

        public static void SetSession(UsersInfo usersInfo)
        {
            UserId = usersInfo.UserId;
            UserName = usersInfo.UserName;
        }

    }
}
