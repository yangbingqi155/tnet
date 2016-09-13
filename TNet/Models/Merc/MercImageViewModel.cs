using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TNet.EF;

namespace TNet.Models
{
    [NotMapped]
    public class MercImageViewModel:MercImage
    {
        [Display(Name ="图片编号")]
        public int MercImageId { get; set; }

        public int idmerc { get; set; }

        [Display(Name ="产品图片")]
        [StringLength(500)]
        public string Path { get; set; }

        [Display(Name ="排序")]
        public int? SortID { get; set; }

        [Display(Name ="是否启用")]
        public bool InUse { get; set; }

        public void CopyFromBase(MercImage mercImage)
        {
            this.MercImageId = mercImage.MercImageId;
            this.idmerc = mercImage.idmerc;
            this.Path = mercImage.Path;
            this.SortID = mercImage.SortID;
            this.InUse = mercImage.InUse;
            
        }

        public void CopyToBase(MercImage mercImage)
        {
            mercImage.MercImageId = this.MercImageId;
            mercImage.idmerc = this.idmerc;
            mercImage.Path = this.Path;
            mercImage.SortID = this.SortID;
            mercImage.InUse = this.InUse;
        }
    }
}