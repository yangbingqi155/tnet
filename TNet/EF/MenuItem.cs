namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuItem")]
    public partial class MenuItem
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        public int? SortID { get; set; }

        public int? ParentID { get; set; }

        [StringLength(500)]
        public string Link { get; set; }
    }
}
