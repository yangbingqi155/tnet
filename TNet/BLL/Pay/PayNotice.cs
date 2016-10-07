using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.Msg;
using TNet.Models.Pay;
using WxPayAPI;

namespace TNet.BLL.Pay
{
    public class PayNotice
    {
        public static int Press()
        {
            int result = Notify.Press();
            if (result == PayNotifyResult.Pay)
            {
                
            }
            return result;

        }

        public static void ResponsePayPressResult(string code, string msg = "")
        {
            Notify.ResponsePayPressResult(code, msg);

        }
    }
}