namespace WeChatApp.Models
{
    public class AccessTokenM
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string access_token { get; set; }


        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public string Expires_in { get; set; }
    }
}