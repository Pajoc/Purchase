using Purchase.Model;
using Purchase.UI.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {

        public MainViewModel(INavigationViewModel navigationViewModel, ISupplierDetailViewModel supplierDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            SupplierDetailViewModel = supplierDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        //Os set são colocados diretamente no construtor
        public INavigationViewModel NavigationViewModel { get; }
        public ISupplierDetailViewModel SupplierDetailViewModel { get; }
    }
   
}
