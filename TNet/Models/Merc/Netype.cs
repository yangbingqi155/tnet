using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNet.Models
{
    /// <summary>
    /// 接入方式:光纤
    /// </summary>
    public enum Netype
    {
        /// <summary>
        /// 光纤
        /// </summary>
        [Display(Name ="光纤")]
        Optical=0,

        /// <summary>
        /// 非光纤
        /// </summary>
        [Display(Name = "非光纤")]
        NoOptical = 1
    }
}