using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.Models
{
    public class Message
    {
        public string SendClientId { get; set; }

        public string Action { get; set; }

        public string Msg { get; set; }

        public string NickName { get; set; }
    }
}
