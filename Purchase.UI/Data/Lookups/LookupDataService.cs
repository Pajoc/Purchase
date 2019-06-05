using Purchase.DataAccess;
using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Loockups
{
    public class LookupDataService : ILookupSupplierDataService, ISupplierTypeLookupDataService
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

        public async Task<List<LookupSupplier>> GetSupplierTypeLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.SupplierTypes.AsNoTracking().Select(s => new LookupSupplier
                {
                    Id = s.Id,
                    DisplayMember = s.Type
                }).ToListAsync();
            }

        }
    }
}
