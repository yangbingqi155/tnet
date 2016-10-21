namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SetupAddr")]
    public partial class SetupAddr
    {
        [Key]
        [StringLength(60)]
        public string idaddr { get; set; }

        [Required]
        [StringLength(60)]
        public string idtype { get; set; }

        [Required]
        [StringLength(60)]
        public string idsetup { get; set; }

        [Required]
        [StringLength(150)]
        public string addr { get; set; }

        [StringLength(12)]
        public string phone { get; set; }

        [StringLength(30)]
        public string service { get; set; }

        [StringLength(10)]
        public string notes { get; set; }

        [StringLength(50)]
        public string acceptime { get; set; }

        [StringLength(60)]
        public string setuptime { get; set; }

        public bool inuse { get; set; }
    }
}
