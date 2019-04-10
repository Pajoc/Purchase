using Purchase.Model;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
