using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Event;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<ISupplierDetailViewModel> _supplierDetailViewModelCreator;
        private ISupplierDetailViewModel _supplierDetailViewModel;

        //public MainViewModel(INavigationViewModel navigationViewModel, ISupplierDetailViewModel supplierDetailViewModel,
        public MainViewModel(INavigationViewModel navigationViewModel, Func<ISupplierDetailViewModel> supplierDetailViewModelCreator,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _supplierDetailViewModelCreator = supplierDetailViewModelCreator;

            _eventAggregator.GetEvent<OpenSupplierDtlViewEvent>().Subscribe(OnOpenSupplierDetailView);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        

        //Os set são colocados diretamente no construtor
        public INavigationViewModel NavigationViewModel { get; }

        public ISupplierDetailViewModel SupplierDetailViewModel
        {
            get { return _supplierDetailViewModel; }
            private set { _supplierDetailViewModel = value;
                OnpropertyChanged();
            }
        }

        private async void OnOpenSupplierDetailView(int SupplierId)
        {
            SupplierDetailViewModel = _supplierDetailViewModelCreator();
            await SupplierDetailViewModel.LoadAsync(SupplierId);
        }
    }
   
}
