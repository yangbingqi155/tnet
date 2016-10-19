using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNet.Models
{
    /// <summary>
    /// 报装类型
    /// </summary>
    public enum SetupType
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Display(Name = "营业厅报装")]
        Store = 0,

        /// <summary>
        /// 工人
        /// </summary>
        [Display(Name = "网上报装")]
        Online = 1
    }
}