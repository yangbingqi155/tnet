namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MyAddr")]
    public partial class MyAddr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idaddr { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iuser { get; set; }

        [StringLength(60)]
        public string contact { get; set; }

        [StringLength(13)]
        public string phone { get; set; }

        [StringLength(10)]
        public string province { get; set; }

        [StringLength(10)]
        public string city { get; set; }

        [StringLength(120)]
        public string street { get; set; }

        [StringLength(10)]
        public string tag { get; set; }

        public bool? isdv { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public bool? inuse { get; set; }
    }
}
