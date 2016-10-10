namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ManageUser")]
    public partial class ManageUser
    {
        public int ManageUserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string MD5Salt { get; set; }

        [StringLength(15)]
        public string phone { get; set; }

        [StringLength(50)]
        public string idweixin { get; set; }

        public bool? recv_order { get; set; }

        public bool? recv_setup { get; set; }

        public bool? send_setup { get; set; }

        [StringLength(60)]
        public string notes { get; set; }

        public bool inuse { get; set; }

        public int UserType { get; set; }
    }
}
