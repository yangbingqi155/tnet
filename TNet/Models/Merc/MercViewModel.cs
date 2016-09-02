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
    public class MercViewModel:Merc
    {
        [Display(Name = "产品编号")]
        public int idmerc { get; set; }

        [Display(Name = "产品类型")]
        [Required]
        public int? idtype { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        [Display(Name = "产品类型")]
        public string MercTypeName { get; set; }

        [Display(Name = "产品名称")]
        [Required]
        [StringLength(60)]
        public string merc1 { get; set; }

        [Display(Name = "卖点")]
        [StringLength(80)]
        public string sellpt { get; set; }

        [Display(Name ="基本价格")]
        public double? baseprice { get; set; }

        [Display(Name = "销量")]
        public int? sellcount { get; set; }

        [Display(Name = "销售开始时间")]
        public DateTime? stime { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? cretime { get; set; }

        [Display(Name = "销售结束时间")]
        public DateTime? entime { get; set; }

        [Display(Name = "接入方式")]
        public int? netype { get; set; }

        [Display(Name = "图片")]
        [StringLength(255)]
        public string imgs { get; set; }

        [Display(Name = "描述图片集合")]
        [StringLength(255)]
        public string descs { get; set; }

        [Display(Name = "备注")]
        [StringLength(50)]
        public string notes { get; set; }

        [Display(Name = "是否启用")]
        [Required]
        public bool inuse { get; set; }
        
        public List<MercTypeViewModel> mercTypes { get; set; }

        public void CopyFromBase(Merc merc) {
            this.idmerc = merc.idmerc;
            this.idtype = merc.idtype;
            this.merc1 = merc.merc1;
            this.sellpt = merc.sellpt;
            this.baseprice = merc.baseprice;
            this.sellcount = merc.sellcount;
            this.stime = merc.stime;
            this.cretime = merc.cretime;
            this.entime = merc.entime;
            this.netype = merc.netype;
            this.imgs = merc.imgs;
            this.descs = merc.descs;
            this.notes = merc.notes;
            this.inuse = merc.inuse;
        }

        public void CopyToBase(Merc merc) {
            merc.idmerc = this.idmerc;
            merc.idtype = this.idtype;
            merc.merc1 = this.merc1;
            merc.sellpt = this.sellpt;
            merc.baseprice = this.baseprice;
            merc.sellcount = this.sellcount;
            merc.stime = this.stime;
            merc.cretime = this.cretime;
            merc.entime = this.entime;
            merc.netype = this.netype;
            merc.imgs = this.imgs;
            merc.descs = this.descs;
            merc.notes = this.notes;
            merc.inuse = this.inuse;
        }
    }
}