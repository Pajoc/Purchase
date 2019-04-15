using System.Collections.Generic;
using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data
{
    public interface ISupplierDataService
    {
        Task<Supplier> GetByIdAsync(int ID);
        Task SaveAsync(Supplier supplier);
    }
}