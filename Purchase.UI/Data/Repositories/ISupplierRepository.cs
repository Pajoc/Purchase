using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data.Repositories
{
    public interface ISupplierRepository:IGenericRepository<Supplier>
    {
        void RemovePhoneNumber(SupplierPhoneNumber model);
        Task<bool> HasMeetingsAsync(int supplierId);
    }
}