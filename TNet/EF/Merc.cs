namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Merc")]
    public partial class Merc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idmerc { get; set; }

        public int? idtype { get; set; }

        [Column("merc")]
        [Required]
        [StringLength(60)]
        public string merc1 { get; set; }

        [StringLength(80)]
        public string sellpt { get; set; }

        public double? baseprice { get; set; }

        public int? sellcount { get; set; }

        public DateTime? stime { get; set; }

        public DateTime? cretime { get; set; }

        public DateTime? entime { get; set; }

        public int? netype { get; set; }

        [StringLength(255)]
        public string imgs { get; set; }

        [StringLength(255)]
        public string descs { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public bool inuse { get; set; }
    }
}
