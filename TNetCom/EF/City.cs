namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        [Key]
        [StringLength(60)]
        public string idcity { get; set; }

        [Column("city")]
        [Required]
        [StringLength(60)]
        public string city1 { get; set; }

        [StringLength(60)]
        public string notes { get; set; }

        public bool? inuse { get; set; }
    }
}
