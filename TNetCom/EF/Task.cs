namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task
    {
        [Key]
        [StringLength(60)]
        public string idtask { get; set; }

        [StringLength(60)]
        public string idsend { get; set; }

        [StringLength(60)]
        public string orderno { get; set; }

        [StringLength(60)]
        public string send { get; set; }

        public DateTime? cretime { get; set; }

        public DateTime? revctime { get; set; }

        public DateTime? dotime { get; set; }

        public DateTime? finishtime { get; set; }

        public DateTime? echotime { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public int? status { get; set; }

        [StringLength(150)]
        public string title { get; set; }

        [StringLength(200)]
        public string text { get; set; }

        public bool inuse { get; set; }
    }
}
