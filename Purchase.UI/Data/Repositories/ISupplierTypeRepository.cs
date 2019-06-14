using Purchase.Model;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Repositories
{
    public interface ISupplierTypeRepository: IGenericRepository<SupplierType>
    {
        Task<bool> IsReferencedBySupplierAsync(int supplierTypeId);
    }
}