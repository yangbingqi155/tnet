using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatApp.Models
{
    /// <summary>
    /// 关注和取消关注事件模型
    /// </summary>
    public class SubscribeMsgM : ReceiveMsgM
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }



        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

    }
}