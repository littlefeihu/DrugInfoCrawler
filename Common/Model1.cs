namespace Common
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=DrugDB")
        {
        }

        public virtual DbSet<DrugItem> DrugItems { get; set; }
        public virtual DbSet<Production> Productions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
