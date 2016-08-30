using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNet.EF
{
    public partial class ManageUserViewModel:ManageUser
    {
        [Display(Name = "用户名:")]
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required]
        public string Password { get; set; }
    }
}