
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCom.Msg
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public sealed class MsgType
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        public readonly static int CreateOrder = 10;

        /// <summary>
        /// 支付订单
        /// </summary>
        public readonly static int PayFinishOrder = 20;

        /// <summary>
        /// 报装订单
        /// </summary>
        public readonly static int SetupOrder = 30;


        /// <summary>
        /// 快过期订单
        /// </summary>
        public readonly static int ExpirePreOrder = 40;

        /// <summary>
        /// 过期订单
        /// </summary>
        public readonly static int ExpireOrder = 50;
    }
}
