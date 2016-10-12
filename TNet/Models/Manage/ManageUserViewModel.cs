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
    public class ManageUserViewModel : ManageUser
    {
        [Display(Name = "用户名")]
        [StringLength(50)]
        [Required]
        public new string UserName { get; set; }

        [Display(Name = "密码")]
        [Required]
        [StringLength(18)]
        public string ClearPassword { get; set; }

        [Display(Name = "密码")]
        [Required]
        [StringLength(100)]
        public new string Password { get; set; }

        [Display(Name = "MD5盐")]
        [StringLength(100)]
        public new string MD5Salt { get; set; }

        [Display(Name = "手机号")]
        [StringLength(15)]
        public new string phone { get; set; }

        [Display(Name = "微信openid")]
        [StringLength(50)]
        public new string idweixin { get; set; }

        [Display(Name = "接收订单")]
        public new bool recv_order { get; set; }

        [Display(Name = "可否接单")]
        public new bool recv_setup { get; set; }

        [Display(Name = "是否可派单")]
        public new bool send_setup { get; set; }

        [Display(Name = "备注")]
        [StringLength(60)]
        public new string notes { get; set; }

        [Required]
        [Display(Name = "启用")]
        public new bool inuse { get; set; }

        [Display(Name = "管理员类型")]
        /// <summary>
        /// 管理员类型
        /// </summary>
        public new ManageUserType UserType { get; set; }

        [Display(Name = "管理员类型")]
        /// <summary>
        /// 管理员类型
        /// </summary>
        public string UserTypeName
        {
            get
            {
                return AttributeHelper.GetDisplayName<ManageUserType>(UserType);
            }
        }

        public static List<SelectItemViewModel<int>> GetUserTypeSelectItems()
        {
            List<SelectItemViewModel<int>> list = new List<SelectItemViewModel<int>>();
            list.Add(new SelectItemViewModel<int>()
            {
                DisplayText = AttributeHelper.GetDisplayName<ManageUserType>(ManageUserType.ManageUser),
                DisplayValue = (int)ManageUserType.ManageUser
            });
            list.Add(new SelectItemViewModel<int>()
            {
                DisplayText = AttributeHelper.GetDisplayName<ManageUserType>(ManageUserType.Worker),
                DisplayValue = (int)ManageUserType.Worker
            });
            return list;
        }


        public void CopyFromBase(ManageUser user)
        {
            this.ManageUserId = user.ManageUserId;
            this.UserName = user.UserName;
            this.Password = user.Password;
            this.MD5Salt = user.MD5Salt;
            this.phone = user.phone;
            this.idweixin = user.idweixin;
            this.recv_order =  user.recv_order ;
            this.recv_setup =  user.recv_setup ;
            this.send_setup =  user.send_setup ;
            this.notes = user.notes;
            this.inuse = user.inuse;
            this.UserType = (ManageUserType)user.UserType;

        }

        public void CopyToBase(ManageUser user)
        {
            user.ManageUserId = this.ManageUserId;
            user.UserName = this.UserName;
            user.Password = this.Password;
            user.MD5Salt = this.MD5Salt;
            user.phone = this.phone;
            user.idweixin = this.idweixin;
            user.recv_order = this.recv_order;
            user.recv_setup = this.recv_setup;
            user.send_setup = this.send_setup;
            user.notes = this.notes;
            user.inuse = this.inuse;
            user.UserType = (int)this.UserType;

        }


    }
}