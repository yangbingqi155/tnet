using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatApp.Models
{
    /// <summary>
    /// 请求素材列表参数
    /// </summary>
    public class RMaterialListParamM
    {
        public string type { get; set; }

        public int offset { get; set; }

        public int count { get; set; }

    }
}