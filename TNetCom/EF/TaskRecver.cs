namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaskRecver")]
    public partial class TaskRecver
    {
        [Key]
        [StringLength(60)]
        public string idrecver { get; set; }

        [Required]
        [StringLength(60)]
        public string idtask { get; set; }

        [StringLength(60)]
        public string mcode { get; set; }

        [StringLength(60)]
        public string mname { get; set; }

        public DateTime? cretime { get; set; }

        [StringLength(60)]
        public string smcode { get; set; }

        [StringLength(60)]
        public string smname { get; set; }

        public bool inuse { get; set; }
    }
}
