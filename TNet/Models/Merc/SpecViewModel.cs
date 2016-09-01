using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TNet.EF;

namespace TNet.Models
{
    [NotMapped]
    public class SpecViewModel:Spec
    {
        [Display(Name = "产品编号")]
        public int idmerc { get; set; }

        [Display(Name = "规格号")]
        public int idspec { get; set; }

        [Display(Name = "规格")]
        [StringLength(60)]
        public string spec1 { get; set; }

        [Display(Name = "单价")]
        public double? price { get; set; }

        [Display(Name = "销量")]
        public int? sellcount { get; set; }

        [Display(Name = "实际月数")]
        public int? month { get; set; }

        [Display(Name = "单位（月为单位）")]
        public int? unit { get; set; }

        [Display(Name = "上行")]
        public int? up { get; set; }

        [Display(Name = "下行")]
        public int? down { get; set; }

        [Display(Name = "附送月份")]
        public int? attmonth { get; set; }

        [Display(Name = "初装费")]
        public double? stuprice { get; set; }

        [Display(Name = "移机费")]
        public double? moveprice { get; set; }

        [Display(Name = "产品类型家庭、企业")]
        [StringLength(50)]
        public string usertype { get; set; }

        [Display(Name = "备注")]
        [StringLength(100)]
        public string notes { get; set; }

        [Display(Name = "产品编号")]
        public bool inuse { get; set; }
    }
}