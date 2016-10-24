using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNet.Models
{
    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModuleType
    {
        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name ="商品")]
        Merc=1,

        /// <summary>
        /// 商圈
        /// </summary>
        [Display(Name = "商圈")]
        Bussiness =2,

        /// <summary>
        /// 广告
        /// </summary>
        [Display(Name = "广告")]
        Advertise =3,

        /// <summary>
        /// 通知
        /// </summary>
        [Display(Name = "通知")]
        Notice =4
    }
}