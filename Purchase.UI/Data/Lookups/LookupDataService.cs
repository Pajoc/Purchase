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
    public class LookupDataService : ILookupSupplierDataService, ISupplierTypeLookupDataService, IMeetingLookupDataService
    {
        private Func<PurchaseDbContext> _contextCreator;

        public LookupDataService(Func<PurchaseDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<LookupItem>> GetSupplierLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return  await ctx.Suppliers.AsNoTracking().Select(s => new LookupItem
                {
                    Id = s.Id,
                    DisplayMember = s.Name
                }).ToListAsync();
            }

        }

        public async Task<List<LookupItem>> GetSupplierTypeLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.SupplierTypes.AsNoTracking().Select(s => new LookupItem
                {
                    Id = s.Id,
                    DisplayMember = s.Type
                }).ToListAsync();
            }

        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                    .Select(m =>
                    new LookupItem
                    {
                        Id = m.Id,
                        DisplayMember = m.Title
                    })
                    .ToListAsync();
                return items;
            }
        }
    }
}
