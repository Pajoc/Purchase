using Prism.Commands;
using Prism.Events;
using Purchase.UI.Event;
using Purchase.UI.View.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<ISupplierDetailViewModel> _supplierDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;
        private IDetailViewModel _detailViewModel;

        //public MainViewModel(INavigationViewModel navigationViewModel, ISupplierDetailViewModel supplierDetailViewModel,
        public MainViewModel(INavigationViewModel navigationViewModel, Func<ISupplierDetailViewModel> supplierDetailViewModelCreator,
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _supplierDetailViewModelCreator = supplierDetailViewModelCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenDtlViewEvent>().Subscribe(OnOpenDetailView);

            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            NavigationViewModel = navigationViewModel;
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewDetailCommand { get; }

        //Os set são colocados diretamente no construtor
        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set { _detailViewModel = value;
                OnpropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDtlViewEventArgs args)
        {
            if(DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }


            switch (args.ViewModelName)
            {
                case nameof(SupplierDetailViewModel):
                    DetailViewModel = _supplierDetailViewModelCreator();
                    break;
            }
            
            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDtlViewEventArgs { ViewModelName = viewModelType.Name });
        }


        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }

    }

}
