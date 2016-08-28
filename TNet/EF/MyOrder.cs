namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MyOrder")]
    public partial class MyOrder
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int orderno { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iduser { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idpro { get; set; }

        [StringLength(60)]
        public string pro { get; set; }

        public double? price { get; set; }

        [StringLength(100)]
        public string addr { get; set; }

        public DateTime? cretime { get; set; }

        public DateTime? stime { get; set; }

        public DateTime? entime { get; set; }

        public int? otype { get; set; }

        public int? status { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public int? inuse { get; set; }
    }
}
