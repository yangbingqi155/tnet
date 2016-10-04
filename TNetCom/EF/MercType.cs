namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MercType")]
    public partial class MercType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idtype { get; set; }

        [Required]
        [StringLength(60)]
        public string name { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public int? sortno { get; set; }

        public bool inuse { get; set; }
    }
}
