using Purchase.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Purchase.DataAccess
{
    public class PurchaseDbContext: DbContext
    {
        public PurchaseDbContext():base("PurchaseProjDb")
        {

        }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SupplierType> SupplierTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        private void myseed()
        {
            var sup = new List<Supplier>()
            {
                new Supplier { Name = "Irmãos Valente", Code = "IRV" },
                new Supplier { Name = "Acebron", Code = "ACE" },
                new Supplier { Name = "Mecânica exata", Code = "EXA" },
                new Supplier { Name = "Fedex", Code = "FEX" },
                new Supplier { Name = "Embal segur", Code = "EBS" },
            };
            Suppliers.AddRange(sup);
            SaveChanges();
        }
    }
}
