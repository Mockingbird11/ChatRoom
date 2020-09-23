using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharRoom.Entity.CommonModels;
using ChatRoom.DataAcces.Log4Net;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Controllers
{
    public class BaseController : Controller
    {
        public Logger logger = new Logger();
        public ResultModel resultModel = new ResultModel();
    }
}
