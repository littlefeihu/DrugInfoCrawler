namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production")]
    public partial class Production
    {
        public int ID { get; set; }

        [StringLength(550)]
        public string ProductionName { get; set; }

        public DateTime? LSST { get; set; }

        public int FromPage { get; set; }

        public bool Done { get; set; }
    }
}
