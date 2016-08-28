using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatApp.Models
{
    public class ReceiveMsgM
    {

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者openid
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }


        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }       

   
    }
}