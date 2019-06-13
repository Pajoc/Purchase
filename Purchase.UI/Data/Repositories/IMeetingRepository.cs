using System.Collections.Generic;
using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data.Repositories
{
    public interface IMeetingRepository: IGenericRepository<Meeting>
    {
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task ReloadSupplierAsync(int supplierId);
    }
}