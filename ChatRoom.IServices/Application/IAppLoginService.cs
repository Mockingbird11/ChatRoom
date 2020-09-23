using CharRoom.Entity;
using ChatRoom.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.IServices
{
    public interface IAppLoginService : ICommonServices<UsersInfo>, IRedisServices
    {
        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <returns></returns>
        Task<UsersInfo> IsUserExist(string userName, string passWord);

        Task<bool> IsUserExist(string userName);
    }
}
