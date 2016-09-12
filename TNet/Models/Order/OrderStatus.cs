using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Order
{
    /// <summary>
    /// 订单状态
    /// 
    /// 支付完成之前可以取消订单
    /// </summary>
    public class OrderStatus
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        public static readonly int WaitPay = 10;


        /// <summary>
        /// 支付完成
        /// </summary>
        public static readonly int PayFinish = 20;


        /// <summary>
        /// 等待结算
        /// </summary>
        public static readonly int WaitSettle = 30;

        /// <summary>
        /// 结算完成
        /// </summary>
        public static readonly int SettleFinish = 40;

        /// <summary>
        /// 申请退款
        /// </summary>
        public static readonly int ApplyReFund = 50;

        /// <summary>
        /// 申请退款失败
        /// </summary>
        public static readonly int ApplyReFundFail = 60;

        /// <summary>
        /// 退款中...
        /// </summary>
        public static readonly int ReFund = 70;

        /// <summary>
        /// 退款完成
        /// </summary>
        public static readonly int ReFundFinish = 80;

        /// <summary>
        /// 交易完成
        /// </summary>
        public static readonly int Finish = 99;

        /// <summary>
        /// 交易取消
        /// </summary>
        public static readonly int Cancel = -99;

        public static List<OrderStatusItem> s = new List<OrderStatusItem>()
        {
            new OrderStatusItem()
            {
                status = WaitPay,
                text = "等待支付",
                ops = "cancel|pay"
            },
            new OrderStatusItem()
            {
                status = PayFinish,
                text = "支付完成",
                ops = "sendmsg"
            }
            ,
            new OrderStatusItem()
            {
                status = WaitSettle,
                text = "等待结算",
                ops = "sendmsg"
            }


            ,
            new OrderStatusItem()
            {
                status = Cancel,
                text = "已经取消",
                ops = "sendmsg"
            }
        };
        public static List<OrderStatusItem> get()
        {
            return s;
        }
    }
}