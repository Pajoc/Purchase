using System.Collections.Generic;
using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data
{
    public interface ISupplierDataService
    {
        Task<List<Supplier>> GetAllAsync();
    }
}