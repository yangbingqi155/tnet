using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace TNetService.BLL
{
    public class Msg
    {
        public static string sendMsg(string data)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + Pub.accessToken;
            Pub.e("sendMsg-url=" + url);
            string m = HttpHelp.Post(url, data);
            Pub.e("sendMsg-result=" + m);
            return m;

        }
    }
}
