using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNet.BLL.User;

namespace TNet.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            ViewBag.ok =  UserBll.Auth2();
            return View();
        }
    }
}