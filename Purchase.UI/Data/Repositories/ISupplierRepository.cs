using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetByIdAsync(int ID);
        Task SaveAsync();
        bool HasChanges();
        void Add(Supplier supplier);
        void Remove(Supplier supplier);
       
    }
}