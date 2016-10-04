namespace TNetCom
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Setup")]
    public partial class Setup
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(60)]
        public string idsetup { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(60)]
        public string idtype { get; set; }

        [Column("setup")]
        [StringLength(160)]
        public string setup1 { get; set; }

        [StringLength(150)]
        public string resource { get; set; }

        [StringLength(120)]
        public string setuptype { get; set; }

        [StringLength(10)]
        public string notes { get; set; }

        public bool? inuse { get; set; }
    }
}
