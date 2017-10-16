namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cnki")]
    public partial class Cnki
    {
        public int ID { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string JournalName { get; set; }
        public string PublishDate { get; set; }
        public int DownloadedCount { get; set; }


        public string KeyString { get; set; }

        public string DataString { get; set; }

        public string KeyWord { get; set; }

        public string Category { get; set; }



    }
}
