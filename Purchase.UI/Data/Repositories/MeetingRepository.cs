using Purchase.DataAccess;
using Purchase.Model;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;

namespace Purchase.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, PurchaseDbContext>, IMeetingRepository
    {
        public MeetingRepository(PurchaseDbContext context) : base (context)
        {

        }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(m => m.Suppliers)
                .SingleAsync(m => m.Id == id);
        }


        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            //return await Context.Set<Supplier>()
            //    .ToListAsync();

            return await Context.Suppliers.ToListAsync();
        }
    }
}
