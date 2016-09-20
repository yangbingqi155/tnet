namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MyOrderPress")]
    public partial class MyOrderPress
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(60)]
        public string idpress { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(60)]
        public string orderno { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int status { get; set; }

        [StringLength(60)]
        public string statust { get; set; }

        public DateTime? cretime { get; set; }

        [StringLength(50)]
        public string oper { get; set; }

        public bool? inuse { get; set; }
    }
}
