using Purchase.DataAccess;
using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.Data
{
    public class LookupDataService : ILookupSupplierDataService
    {
        private Func<PurchaseDbContext> _contextCreator;

        public LookupDataService(Func<PurchaseDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<LookupSupplier>> GetSupplierLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return  await ctx.Suppliers.AsNoTracking().Select(s => new LookupSupplier
                {
                    Id = s.Id,
                    DisplayMember = s.Name
                }).ToListAsync();
            }

        }
    }
}
