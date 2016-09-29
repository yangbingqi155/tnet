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
        /// 创建订单
        /// </summary>
        public static readonly int Create = 1;

        /// <summary>
        /// 确认订单
        /// </summary>
        public static readonly int Confirm = 2;

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
        
        public static Dictionary<int, OrderStatusItem> s = new Dictionary<int, OrderStatusItem>()
        {
             {
                Create,
                new OrderStatusItem()
                {
                    text = "订单创建",
                    ops = "cancel|pay"
                }
            },
            {
                Confirm,
                new OrderStatusItem()
                {
                    text = "确认订单",
                    ops = ""
                }
            },
            {
                WaitPay,
                new OrderStatusItem()
                {
                    text = "等待支付",
                    ops = "cancel|pay"
                }
            },
            {
                PayFinish,
                new OrderStatusItem()
                {
                    text = "支付完成",
                    ops = "sendmsg"
                }
            },
            {
                WaitSettle,
                new OrderStatusItem()
                {
                    text = "等待结算",
                    ops = "sendmsg"
                }
            },
            {
                Cancel,
                new OrderStatusItem()
                {
                    text = "已经取消",
                    ops = ""
                }
            },
            {
                0,
                new OrderStatusItem()
                {
                    text = "未知",
                    ops = ""
                }
            }
        };
        public static Dictionary<int, OrderStatusItem> get()
        {
            return s;
        }

        public static OrderStatusItem get(int status)
        {
            return s.ContainsKey(status) ? s[status] : s[0];
        }

        public static List<SelectItemViewModel<int>> GetSelectItems() {
            List<SelectItemViewModel<int>> list = new List<SelectItemViewModel<int>>();
            list.Add(new SelectItemViewModel<int>() {
                DisplayText = "所有订单状态",
                DisplayValue=0
            });
            foreach (var item in s)
            {
                if (item.Key!=0) {
                    SelectItemViewModel<int> model = new SelectItemViewModel<int>();
                    model.DisplayValue = item.Key;
                    model.DisplayText = item.Value.text;
                    list.Add(model);
                }
               
            }
            return list;
        }
    }
}