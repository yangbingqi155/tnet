using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TNet.EF;


namespace TNet.Models
{
    [NotMapped]
    public class MercTypeViewModel:MercType
    {
        [Display(Name = "类型编号")]
        public int idtype { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "类型名称")]
        public string name { get; set; }

        [StringLength(50)]
        [Display(Name = "备注")]
        public string notes { get; set; }
        
        [Display(Name = "启用")]
        [Required]
        public bool inuse { get; set; }

        public void CopyFromBase(MercType mercType)
        {
            this.idtype = mercType.idtype;
            this.name = mercType.name;
            this.notes = mercType.notes;
            this.inuse = mercType.inuse;
        }

        public void CopyToBase(MercType mercType)
        {
            mercType.idtype = this.idtype;
            mercType.name = this.name;
            mercType.notes = this.notes;
            mercType.inuse = this.inuse;
           
        }
    }
}