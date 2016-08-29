using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace TNet.EF
{
    [Table("ManageUser")]
    public partial class ManageUser
    {
        [Key]
        [Column(Order =0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ManageUserId { get; set; }

        [Display(Name ="用户名:")]
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [StringLength(50)]
        [Required]
        public string Password { get; set; }
    }
}