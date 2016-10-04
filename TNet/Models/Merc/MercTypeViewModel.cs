using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;


namespace TNet.Models
{
    [NotMapped]
    public   class MercTypeViewModel:MercType
    {
        [Display(Name = "类型编号")]
        public new  int idtype { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "类型名称")]
        public new  string name { get; set; }

        [StringLength(50)]
        [Display(Name = "备注")]
        public new  string notes { get; set; }

        [Display(Name = "排序")]
        public new  int? sortno { get; set; }

        [Display(Name = "启用")]
        [Required]
        public new  bool inuse { get; set; }

        public   void CopyFromBase(MercType mercType)
        {
            this.idtype = mercType.idtype;
            this.name = mercType.name;
            this.notes = mercType.notes;
            this.sortno = mercType.sortno;
            this.inuse = mercType.inuse;
        }

        public   void CopyToBase(MercType mercType)
        {
            mercType.idtype = this.idtype;
            mercType.name = this.name;
            mercType.notes = this.notes;
            mercType.sortno = this.sortno;
            mercType.inuse = this.inuse;
        }
    }
}