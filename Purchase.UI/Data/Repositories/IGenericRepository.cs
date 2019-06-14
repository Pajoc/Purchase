using System.Collections.Generic;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        
        Task<T> GetByIdAsync(int ID);
        Task<IEnumerable<T>> GetAllAsync();
        Task SaveAsync();

        void Add(T model);
        bool HasChanges();
        void Remove(T model);
        
    }
}