using CharRoom.Entity;
using ChatRoom.DataAcces.Redis;
using ChatRoom.IServices;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Services.Application
{
    public class AppLoginService : CommonService<UsersInfo>, IAppLoginService
    {
        private IDatabase redisDb;
        public AppLoginService() : base()
        {
            redisDb = new RedisHelper().GetDatabase();
        }
        public Task<UsersInfo> IsUserExist(string userName, string passWord)
        {
            return Task.Run(() => _db.Queryable<UsersInfo>().Where(x => x.UserName == userName && x.UserPassWord == passWord).First());
        }

        public Task<bool> IsUserExist(string userName)
        {
            return Task.Run(() => _db.Queryable<UsersInfo>().Where(x => x.UserName == userName).Count() > 0);
        }

        public object GetCacheByKey(string key)
        {
            return redisDb.StringGet(key);
        }

    }
}
