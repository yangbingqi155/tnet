namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CityRelation")]
    public partial class CityRelation
    {
        [Key]
        [StringLength(50)]
        public string idrelation { get; set; }

        [StringLength(50)]
        public string idcity { get; set; }

        [StringLength(50)]
        public string idmodule { get; set; }

        public int? moduletype { get; set; }

        public bool? inuse { get; set; }
    }
}
