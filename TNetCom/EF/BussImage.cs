namespace TCom.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BussImage")]
    public partial class BussImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long BussImageId { get; set; }

        public long? idbuss { get; set; }

        [StringLength(500)]
        public string Path { get; set; }

        public int? SortID { get; set; }

        public bool? InUse { get; set; }
    }
}
