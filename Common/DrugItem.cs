namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DrugItem")]
    public partial class DrugItem
    {
        public int ID { get; set; }

        [StringLength(550)]
        public string DrugName { get; set; }

        [StringLength(550)]
        public string PiZhunWenHao { get; set; }

        [StringLength(550)]
        public string Manufacturer { get; set; }

        [StringLength(50)]
        public string DosageForm { get; set; }

        [StringLength(550)]
        public string Specification { get; set; }

        [StringLength(550)]
        public string DrugUrl { get; set; }

        [StringLength(550)]
        public string CompanyUrl { get; set; }

        public DateTime? LSST { get; set; }
    }
}
