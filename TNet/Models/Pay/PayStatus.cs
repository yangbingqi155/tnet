using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Pay
{
    public sealed class PayStatus
    {
        public static readonly string WaitPay = "";//等待支付
        public static readonly string WeiXin_SUCCESS = "SUCCESS";//支付成功
        public static readonly string WeiXin_REFUND = "REFUND";//转入退款
        public static readonly string WeiXin_NOTPAY = "NOTPAY";//未支付
        public static readonly string WeiXin_CLOSED = "CLOSED";//已关闭
        public static readonly string WeiXin_REVOKED = "REVOKED";//已撤销（刷卡支付）
        public static readonly string WeiXin_USERPAYING = "USERPAYING";//用户支付中
        public static readonly string WeiXin_PAYERROR = "PAYERROR";//支付失败(其他原因，如银行返回失败)


    }
}