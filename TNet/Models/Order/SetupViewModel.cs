using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;
using TNet.Util;

namespace TNet.Models
{
    [NotMapped]
    public class SetupViewModel:Setup
    {
        [Required]
        [Display(Name = "安装编号")]
        [StringLength(60)]
        public new string idsetup { get; set; }

        [Required]
        [Display(Name = "类型编号")]
        [StringLength(60)]
        public new string idtype { get; set; }
        
        [Display(Name = "安装标题")]
        [StringLength(160)]
        public new string setup1 { get; set; }

        [Display(Name = "所需资源")]
        [StringLength(150)]
        public new string resource { get; set; }

        [Display(Name = "报装方式")]
        [StringLength(120)]
        public new string setuptype { get; set; }

        [Display(Name = "备注")]
        [StringLength(10)]
        public new string notes { get; set; }

        [Display(Name = "启用")]
        public new bool inuse { get; set; }

        [Display(Name = "产品类型")]
        public MercTypeViewModel merctype { get; set; }

        [Display(Name = "产品类型")]
        public string merctypename
        {
            get
            {
                return merctype == null ? "" : merctype.name;
            }
        }

        //public static List<SelectItemViewModel<int>> GetSetupTypeSelectItems()
        //{
        //    List<SelectItemViewModel<int>> list = new List<SelectItemViewModel<int>>();
        //    list.Add(new SelectItemViewModel<int>()
        //    {
        //        DisplayText = AttributeHelper.GetDisplayName<SetupType>(SetupType.Store),
        //        DisplayValue = (int)SetupType.Store
        //    });
        //    list.Add(new SelectItemViewModel<int>()
        //    {
        //        DisplayText = AttributeHelper.GetDisplayName<SetupType>(SetupType.Online),
        //        DisplayValue = (int)SetupType.Online
        //    });
        //    return list;
        //}

        public void CopyFromBase(Setup setup)
        {
            this.idsetup = setup.idsetup;
            this.idtype = setup.idtype;
            this.setup1 = setup.setup1;
            this.resource = setup.resource;
            this.setuptype = setup.setuptype;
            this.notes = setup.notes;
            this.inuse = setup.inuse;
        }

        public void CopyToBase(Setup setup)
        {
            setup.idsetup = this.idsetup;
            setup.idtype = this.idtype;
            setup.setup1 = this.setup1;
            setup.resource = this.resource;
            setup.setuptype = this.setuptype;
            setup.notes = this.notes;
            setup.inuse = this.inuse;
        }
    }
}