namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Business")]
    public partial class Business
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idbuss { get; set; }

        [Required]
        [StringLength(60)]
        public string buss { get; set; }

        [StringLength(100)]
        public string addr { get; set; }

        [StringLength(60)]
        public string sellpt { get; set; }

        [StringLength(255)]
        public string imgs { get; set; }

        public DateTime? cretime { get; set; }

        public DateTime? busstime { get; set; }

        public int? blevel { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public bool? inuse { get; set; }
    }
}
