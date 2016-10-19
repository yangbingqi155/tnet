using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;

namespace TNet.Models
{
    [NotMapped]
    public class SetupAddrViewModel:SetupAddr
    {
        [Display(Name = "办理点编号")]
        [StringLength(60)]
        [Required]
        public string idaddr { get; set; }

        [Display(Name = "类型编号")]
        [StringLength(60)]
        [Required]
        public string idtype { get; set; }

        [Display(Name = "安装编号")]
        [StringLength(60)]
        [Required]
        public string idsetup { get; set; }

        [Display(Name = "办理地点")]
        [Required]
        [StringLength(150)]
        public string addr { get; set; }

        [Display(Name = "联系电话")]
        [StringLength(12)]
        public string phone { get; set; }

        [Display(Name = "服务热线")]
        [StringLength(30)]
        public string service { get; set; }

        [Display(Name = "备注")]
        [StringLength(10)]
        public string notes { get; set; }

        [Display(Name = "办理时间")]
        [StringLength(50)]
        public string acceptime { get; set; }

        [Display(Name = "安装时间")]
        [StringLength(60)]
        public string setuptime { get; set; }

        [Display(Name = "启用")]
        public bool inuse { get; set; }

        [Display(Name ="报装")]
        public SetupViewModel setup { get; set; }

        [Display(Name = "产品类型")]
        public MercTypeViewModel merctype { get; set; }

        public void CopyFromBase(SetupAddr setupAddr)
        {
            this.idaddr = setupAddr.idaddr;
            this.idtype = setupAddr.idtype;
            this.idsetup = setupAddr.idsetup;
            this.addr = setupAddr.addr;
            this.phone = setupAddr.phone;
            this.service = setupAddr.service;
            this.notes = setupAddr.notes;
            this.acceptime = setupAddr.acceptime;
            this.setuptime = setupAddr.setuptime;
            this.inuse = setupAddr.inuse;
        }

        public void CopyToBase(SetupAddr setupAddr)
        {
            setupAddr.idaddr = this.idaddr;
            setupAddr.idtype = this.idtype;
            setupAddr.idsetup = this.idsetup;
            setupAddr.addr = this.addr;
            setupAddr.phone = this.phone;
            setupAddr.service = this.service;
            setupAddr.notes = this.notes;
            setupAddr.acceptime = this.acceptime;
            setupAddr.setuptime = this.setuptime;
            setupAddr.inuse = this.inuse;
        }
    }
}