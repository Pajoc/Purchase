using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public interface ISupplierDetailViewModel
    {
        Task LoadAsync(int SupID);
    }
}