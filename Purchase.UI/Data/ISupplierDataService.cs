using System.Collections.Generic;
using Purchase.Model;

namespace Purchase.UI.Data
{
    public interface ISupplierDataService
    {
        IEnumerable<Supplier> GetAll();
    }
}