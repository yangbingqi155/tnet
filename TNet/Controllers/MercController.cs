using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TNet.Controllers
{
    public class MercController : Controller
    {
        /// <summary>
        /// 宽带
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string tag)
        {
            ViewBag.tag = tag;
            if (tag == "Setup")
            {
                ViewBag.Title = "宽带报装";
            }else
            {
                ViewBag.Title = "T网宽带";
            }
            return View();
        }

        /// <summary>
        /// 商品明细
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return View();
        }

    }
}