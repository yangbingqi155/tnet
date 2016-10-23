namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdvertiseType")]
    public partial class AdvertiseType
    {
        [Key]
        [StringLength(50)]
        public string idat { get; set; }

        [StringLength(100)]
        public string typename { get; set; }

        public int? sortno { get; set; }

        public DateTime? createtime { get; set; }

        public bool inuse { get; set; }
    }
}
