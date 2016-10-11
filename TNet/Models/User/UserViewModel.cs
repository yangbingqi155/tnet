using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;

namespace TNet.Models {
    public class UserViewModel: TCom.EF.User {
        [Display(Name = "微信openid")]
        [StringLength(50)]
        public string idweixin { get; set; }

        [Display(Name = "用户编号")]
        public long iduser { get; set; }

        [Display(Name = "用户名称")]
        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? cretime { get; set; }

        [Display(Name = "电话号码")]
        [StringLength(15)]
        public string phone { get; set; }

        [Display(Name = "公司")]
        [StringLength(60)]
        public string comp { get; set; }

        [Display(Name = "性别")]
        public int? sex { get; set; }

        [Display(Name = "头像地址")]
        [StringLength(255)]
        public string avatar { get; set; }

        [Display(Name = "备注")]
        [StringLength(50)]
        public string notes { get; set; }

        [Display(Name = "启用")]
        public bool inuse { get; set; }

        public void CopyFromBase(TCom.EF.User user) {
            this.idweixin = user.idweixin;
            this.iduser = user.iduser;
            this.name = user.name;
            this.cretime = user.cretime;
            this.phone = user.phone;
            this.comp = user.comp;
            this.sex = user.sex;
            this.avatar = user.avatar;
            this.notes = user.notes;
            this.inuse = user.inuse;
        }

        public void CopyToBase(TCom.EF.User user) {
            user.idweixin = this.idweixin;
            user.iduser = this.iduser;
            user.name = this.name;
            user.cretime = this.cretime;
            user.phone = this.phone;
            user.comp = this.comp;
            user.sex = this.sex;
            user.avatar = this.avatar;
            user.notes = this.notes;
            user.inuse = this.inuse;
        }
    }
}