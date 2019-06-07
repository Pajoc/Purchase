namespace Purchase.DataAccess.Migrations
{
    using Purchase.Model;
    using System;
    using System.Collections.Generic;
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

            context.SaveChanges();

            context.SupplierPhoneNumbers.AddOrUpdate(pn => pn.Number,
                new SupplierPhoneNumber { Number = "+351 258721050", SupplierId = context.Suppliers.First().Id });

            context.Meetings.AddOrUpdate(m => m.Title,
                new Meeting
                {
                    Title = "Project xpto",
                    DateFrom = new DateTime(2019, 7, 10),
                    DateTo = new DateTime(2019, 7, 10),
                    Suppliers = new List<Supplier>
                    {
                        context.Suppliers.Single(s => s.Name == "Acebron"),
                        context.Suppliers.Single(s => s.Name == "Fedex"),
                    }
                });

        }
    }
}
