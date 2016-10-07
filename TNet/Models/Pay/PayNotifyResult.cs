using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Pay
{
    /// <summary>
    /// 支付通知结果
    /// </summary>
    public class PayNotifyResult
    {
        /// <summary>
        /// 支付完成
        /// </summary>
        public readonly static int Pay = 10;

        /// <summary>
        /// 已经支付完成
        /// </summary>
        public readonly static int Payed = 20;

        /// <summary>
        /// 支付失败
        /// </summary>
        public readonly static int Fail = 30;
    }
}