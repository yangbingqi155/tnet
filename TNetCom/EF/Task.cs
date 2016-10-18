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

        [Required]
        [StringLength(60)]
        public string iduser { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [StringLength(60)]
        public string idsend { get; set; }

        [StringLength(60)]
        public string orderno { get; set; }

        [StringLength(60)]
        public string send { get; set; }

        public DateTime? accpeptime { get; set; }

        public DateTime? cretime { get; set; }

        public DateTime? revctime { get; set; }

        public DateTime? dotime { get; set; }

        public DateTime? finishtime { get; set; }

        public DateTime? echotime { get; set; }

        public int? status { get; set; }

        [StringLength(50)]
        public string contact { get; set; }

        [StringLength(100)]
        public string addr { get; set; }

        [StringLength(13)]
        public string phone { get; set; }

        [StringLength(150)]
        public string title { get; set; }

        [StringLength(200)]
        public string text { get; set; }

        public int? tasktype { get; set; }

        public int? score { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public bool inuse { get; set; }
    }
}
