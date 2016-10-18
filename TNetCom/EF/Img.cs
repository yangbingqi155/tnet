namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Img")]
    public partial class Img
    {
        [Key]
        [StringLength(60)]
        public string idimg { get; set; }

        [StringLength(60)]
        public string outkey { get; set; }

        [StringLength(60)]
        public string outpro { get; set; }

        public bool? inuse { get; set; }

        [StringLength(255)]
        public string path { get; set; }

        public int? sortno { get; set; }
    }
}
