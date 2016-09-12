namespace TNet.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MercImage")]
    public partial class MercImage
    {
        public int MercImageId { get; set; }

        public int idmerc { get; set; }

        [StringLength(500)]
        public string Path { get; set; }

        public int? SortID { get; set; }

        public bool InUse { get; set; }
    }
}
