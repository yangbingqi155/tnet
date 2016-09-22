using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Order
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public class OrderType
    {
        /// <summary>
        /// 商品订单
        /// </summary>
        public static int Merc = 1;


        /// <summary>
        /// 报装业务
        /// </summary>
        public static int Setup = 2;
    }
}