using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace TNet.BLL.Pay
{
    public class PayNoticeBll
    {
        public static void Press()
        {
            Notify bll = new Notify();
            bll.Press();
        }
    }
}