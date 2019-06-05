using System.Collections.Generic;
using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data.Loockups
{
    public interface ISupplierTypeLookupDataService
    {
        Task<List<LookupItem>> GetSupplierTypeLookupAsync();
    }
}