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
            string user = "null";
            bool ok = UserBll.Auth(ref user);
            if (string.IsNullOrWhiteSpace(user))
            {
                user = "null";
            }
            ViewBag.ok = ok;
            ViewBag.ru = "";
            ViewBag.u = new HtmlString(user);
            if (ok)
            {
                string ru = Request.QueryString["ru"];
                if (!string.IsNullOrWhiteSpace(ru))
                {
                    ViewBag.ru = new HtmlString(ru);
                   
                }
            }
            return View();
        }
    }
}