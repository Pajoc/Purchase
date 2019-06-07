using Purchase.DataAccess;
using Purchase.Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Purchase.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, PurchaseDbContext>, IMeetingRepository
    {
        protected MeetingRepository(PurchaseDbContext context) : base(context)
        {
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(m => m.Suppliers)
                .SingleAsync(m => m.Id == id);
        }
    }
}
