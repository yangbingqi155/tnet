using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.Models
{
    [NotMapped]
    public class AdvertiseTypeViewModel:AdvertiseType
    {
        [Required]
        [Display(Name = "广告类型编号")]
        [StringLength(50)]
        public new string idat { get; set; }

        [Required]
        [Display(Name = "类型名称")]
        [StringLength(100)]
        public new string typename { get; set; }

        [Display(Name = "排序")]
        public new int? sortno { get; set; }

        [Display(Name = "创建时间")]
        public new DateTime? createtime { get; set; }

        [Required]
        [Display(Name = "启用")]
        public new bool inuse { get; set; }
        
        public void CopyFromBase(AdvertiseType advertiseType)
        {
            this.idat = advertiseType.idat;
            this.typename = advertiseType.typename;
            this.sortno = advertiseType.sortno;
            this.createtime = advertiseType.createtime;
            this.inuse = advertiseType.inuse;
        }

        public void CopyToBase(AdvertiseType advertiseType)
        {
            advertiseType.idat = this.idat;
            advertiseType.typename = this.typename;
            advertiseType.sortno = this.sortno;
            advertiseType.createtime = this.createtime;
            advertiseType.inuse = this.inuse;
        }
    }
}