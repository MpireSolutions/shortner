using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Controllers
{
    public class BaseController : Controller
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}