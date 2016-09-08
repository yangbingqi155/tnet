using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TNet.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Submit()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Pay(string orderno)
        {
            ViewBag.orderno = orderno;
            return View();
        }
    }
}