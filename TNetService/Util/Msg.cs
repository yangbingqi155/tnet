using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace TNetService.BLL
{
    public sealed class Msg
    {
        public static JObject Send(string data)
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + Pub.accessToken;
                Pub.e("sendMsg-url=" + url);
                string m = HttpHelp.Post(url, data);
                Pub.e("sendMsg-result=" + m);
                JObject j = JObject.Parse(m);
                return j;
            }
            catch (Exception)
            {
            }
            return null;

        }
    }
}
