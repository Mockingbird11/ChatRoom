using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.IServices
{
    public interface IRedisServices
    {
        object GetCacheByKey(string key);
    }
}
