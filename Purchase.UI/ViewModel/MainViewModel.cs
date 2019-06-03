using Prism.Events;
using Purchase.UI.Event;
using Purchase.UI.View.Services;
using System;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<ISupplierDetailViewModel> _supplierDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;
        private ISupplierDetailViewModel _supplierDetailViewModel;

        //public MainViewModel(INavigationViewModel navigationViewModel, ISupplierDetailViewModel supplierDetailViewModel,
        public MainViewModel(INavigationViewModel navigationViewModel, Func<ISupplierDetailViewModel> supplierDetailViewModelCreator,
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _supplierDetailViewModelCreator = supplierDetailViewModelCreator;
            _messageDialogService = messageDialogService;

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
            if(SupplierDetailViewModel != null && SupplierDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            SupplierDetailViewModel = _supplierDetailViewModelCreator();
            await SupplierDetailViewModel.LoadAsync(SupplierId);
        }
    }
   
}
