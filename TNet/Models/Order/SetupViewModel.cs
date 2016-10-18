﻿using System;
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
        [Display(Name = "安装编号")]
        [StringLength(60)]
        public string idsetup { get; set; }

        [Display(Name = "类型编号")]
        [StringLength(60)]
        public string idtype { get; set; }
        
        [Display(Name = "安装标题")]
        [StringLength(160)]
        public string setup1 { get; set; }

        [Display(Name = "所需资源")]
        [StringLength(150)]
        public string resource { get; set; }

        [Display(Name = "报装方式")]
        [StringLength(120)]
        public string setuptype { get; set; }

        [Display(Name = "备注")]
        [StringLength(10)]
        public string notes { get; set; }

        [Display(Name = "启用")]
        public bool inuse { get; set; }

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

        [Display(Name = "报装类型")]
        /// <summary>
        /// 报装类型
        /// </summary>
        public SetupType setupType { get; set; }

        [Display(Name = "报装类型")]
        /// <summary>
        /// 报装类型
        /// </summary>
        public string SetupTypeName
        {
            get
            {
                return AttributeHelper.GetDisplayName<SetupType>(setupType);
            }
        }

        public static List<SelectItemViewModel<int>> GetSetupTypeSelectItems()
        {
            List<SelectItemViewModel<int>> list = new List<SelectItemViewModel<int>>();
            list.Add(new SelectItemViewModel<int>()
            {
                DisplayText = AttributeHelper.GetDisplayName<SetupType>(SetupType.Store),
                DisplayValue = (int)SetupType.Store
            });
            list.Add(new SelectItemViewModel<int>()
            {
                DisplayText = AttributeHelper.GetDisplayName<SetupType>(SetupType.Online),
                DisplayValue = (int)SetupType.Online
            });
            return list;
        }

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