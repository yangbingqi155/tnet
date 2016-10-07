namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Msg")]
    public partial class Msg
    {
        [Key]
        [StringLength(60)]
        public string idmsg { get; set; }

        [StringLength(50)]
        public string idweixin { get; set; }

        [StringLength(60)]
        public string iduser { get; set; }

        [Column("msg")]
        [StringLength(1000)]
        public string msg1 { get; set; }

        public DateTime? cretime { get; set; }

        public DateTime? sendtime { get; set; }

        public DateTime? calltime { get; set; }

        public int? status { get; set; }

        [StringLength(50)]
        public string error { get; set; }

        [StringLength(60)]
        public string orderno { get; set; }

        public int? otype { get; set; }

        public int? type { get; set; }

        public bool? inuse { get; set; }
    }
}
