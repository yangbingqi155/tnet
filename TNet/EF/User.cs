namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string idweixin { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iduser { get; set; }

        [StringLength(50)]
        public string name { get; set; }
        
        public DateTime? cretime { get; set; }

        public bool? isoper { get; set; }

        [StringLength(15)]
        public string phone { get; set; }

        [StringLength(60)]
        public string comp { get; set; }

        public int? sex { get; set; }

        [StringLength(255)]
        public string avatar { get; set; }

        [StringLength(50)]
        public string notes { get; set; }

        public bool? inuse { get; set; }
    }
}
