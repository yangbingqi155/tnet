using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TNet.Controllers
{
    public class BussController : Controller
    {
        // GET: Buss
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Detail(string idbuss)
        {
            ViewBag.idbuss = idbuss;
            return View();
        }
    }
}