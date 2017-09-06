namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GMPInfo")]
    public partial class GMPInfo
    {
        public int ID { get; set; }

        [StringLength(550)]
        public string ManufacturerName { get; set; }

        public DateTime? LSST { get; set; }

        public string GMPNUM { get; set; }

        public string GMPDrugName { get; set; }

        public int ManufacturerID { get; set; }

    }
}
