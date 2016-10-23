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
    public class AdvertiseViewModel:Advertise
    {
        [Display(Name ="广告编号")]
        [StringLength(50)]
        public new string idav { get; set; }

        [Required]
        [Display(Name = "广告类型")]
        [StringLength(50)]
        public new string idat { get; set; }
        
        [Display(Name = "标题")]
        [StringLength(50)]
        public new string title { get; set; }

        [Required]
        [Display(Name = "图片")]
        [StringLength(500)]
        public new string img { get; set; }

        [Display(Name = "链接")]
        [StringLength(500)]
        public new string link { get; set; }

        [Display(Name = "创建时间")]
        public new DateTime? cretime { get; set; }

        [Display(Name = "排序")]
        public new int? sortno { get; set; }

        [Required]
        [Display(Name = "启用")]
        public new bool inuse { get; set; }

        /// <summary>
        /// 广告类型
        /// </summary>
        [Display(Name = "广告类型")]
        public AdvertiseTypeViewModel AdvertiseType { get; set; }

        public void CopyFromBase(Advertise advertise)
        {
            this.idav = advertise.idav;
            this.idat = advertise.idat;
            this.title = advertise.title;
            this.img = advertise.img;
            this.link = advertise.link;
            this.cretime = advertise.cretime;
            this.sortno = advertise.sortno;
            this.inuse = advertise.inuse;
        }

        public void CopyToBase(Advertise advertise)
        {
            advertise.idav = this.idav;
            advertise.idat = this.idat;
            advertise.title = this.title;
            advertise.img = this.img;
            advertise.link = this.link;
            advertise.cretime = this.cretime;
            advertise.sortno = this.sortno;
            advertise.inuse = this.inuse;
        }
    }
}