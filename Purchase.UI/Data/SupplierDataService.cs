using Purchase.DataAccess;
using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Purchase.UI.Data
{
    public class SupplierDataService : ISupplierDataService
    {
        private Func<PurchaseDbContext> _contextCreator;

        public SupplierDataService(Func<PurchaseDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public IEnumerable<Supplier> GetAll()
        {
            //yield return new Supplier { Name = "Irmãos Valente", Code = "IRV" };
            //yield return new Supplier { Name = "Acebron", Code = "ACE" };
            //yield return new Supplier { Name = "Mecânica exata", Code = "EXA" };
            //yield return new Supplier { Name = "Fedex", Code = "FEX" };
            //yield return new Supplier { Name = "Embal segur", Code = "EBS" };


            using (var ctx = _contextCreator())
            {
                return ctx.Suppliers.AsNoTracking().ToList();
            }

        }
    }
}
