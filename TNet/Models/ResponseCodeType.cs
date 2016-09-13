using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models
{
    /// <summary>
    /// 响应代码类型
    /// </summary>
    public enum ResponseCodeType
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 2,

        /// <summary>
        /// 重新登录
        /// </summary>
        LoginAgain = 3,

        /// <summary>
        /// 系统错误
        /// </summary>
        SysError = 4
    }
}