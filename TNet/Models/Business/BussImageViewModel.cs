using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNet.Models {
    [NotMapped]
    public class BussImageViewModel :BussImage{
        [Display(Name ="商圈图片编号")]
        public long BussImageId { get; set; }

        [Display(Name = "商圈编号")]
        public long? idbuss { get; set; }

        [Display(Name = "图片")]
        [StringLength(500)]
        public string Path { get; set; }

        [Display(Name = "排序")]
        public int? SortID { get; set; }

        [Display(Name = "启用")]
        public bool? InUse { get; set; }


        public void CopyFromBase(BussImage img) {
            this.BussImageId = img.BussImageId;
            this.idbuss = img.idbuss;
            this.Path = img.Path;
            this.SortID = img.SortID;
            this.InUse = img.InUse;
        }

        public void CopyToBase(BussImage img) {
            img.BussImageId = this.BussImageId;
            img.idbuss = this.idbuss;
            img.Path = this.Path;
            img.SortID = this.SortID;
            img.InUse = this.InUse;
        }
    }
}