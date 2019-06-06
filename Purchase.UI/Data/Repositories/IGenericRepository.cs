using System.Threading.Tasks;

namespace Purchase.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        void Add(T model);
        Task<T> GetByIdAsync(int ID);
        bool HasChanges();
        void Remove(T model);
        Task SaveAsync();
    }
}