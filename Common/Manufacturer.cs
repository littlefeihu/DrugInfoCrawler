namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Manufacturer")]
    public partial class Manufacturer
    {
        public int ID { get; set; }

        public string ManufacturerName { get; set; }

        public DateTime? LSST { get; set; }

        public string Link { get; set; }


    }
}
