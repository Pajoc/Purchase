namespace Purchase.DataAccess.Migrations
{
    using Purchase.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Purchase.DataAccess.PurchaseDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Purchase.DataAccess.PurchaseDbContext context)
        {
            //campo de controlo
            context.Suppliers.AddOrUpdate(
                 f => f.Code,
                   new Supplier { Name = "Irmãos Valente", Code = "IRV" },
                   new Supplier { Name = "Acebron", Code = "ACE" },
                   new Supplier { Name = "Mecânica exata", Code = "EXA" },
                   new Supplier { Name = "Fedex", Code = "FEX" },
                   new Supplier { Name = "Embal segur", Code = "EBS" }
                  );
            context.SupplierTypes.AddOrUpdate(
                st => st.Type,
                new SupplierType { Type = "Catalog supplier" },
                new SupplierType { Type = "Custom supplier" },
                new SupplierType { Type = "Big supplier" });
        }
    }
}
