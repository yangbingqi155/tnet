namespace WeChatApp.Models
{
    /// <summary>
    /// 验证微信调用
    /// </summary>
    public class signatureM
    {
        /// <summary>
        ///   微信加密签名
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string echostr { get; set; }
    }
}