using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNet.BLL.Pay;
using TNet.Models.Pay;

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



        public ActionResult Detail(string orderno)
        {
            ViewBag.orderno = orderno;
            return View();
        }

        //微信支付通知
        public ActionResult WeixinPayNotice()
        {
            if (PayNotice.Press() != PayNotifyResult.Fail)
            {
                PayNotice.ResponsePayPressResult("SUCCESS", "OK");
            }
            else
            {
                PayNotice.ResponsePayPressResult("FAIL", "ERROR");
            }
            return View();
        }




    }
}