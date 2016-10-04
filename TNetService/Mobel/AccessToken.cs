using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNetService.Mobel
{
    public class AccessToken
    {
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string access_token { get; set; }


        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }

        public DateTime expires { get; set; }

    }
}
