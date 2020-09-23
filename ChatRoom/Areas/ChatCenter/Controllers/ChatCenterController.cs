using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoom.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Areas.ChatCenter.Controllers
{
    [Area("ChatCenter")]
    public class ChatCenterController : BaseController
    {
        public IActionResult ChatCenterIndex()
        {
            return View();
        }
    }
}
