using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using Purchase.UI.Event;
using Purchase.UI.View.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;
        private IMessageDialogService _messageDialogService;
        private IDetailViewModel _selecteddetailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel,IIndex<string,IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _detailViewModelCreator = detailViewModelCreator;
            _messageDialogService = messageDialogService;

            //faz o set
            DetailViewModels = new ObservableCollection<IDetailViewModel>();

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

        //Para poder ter tabs
        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }
        //Passa a ser usado para o detailVM selecionado
        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selecteddetailViewModel; }
            set { _selecteddetailViewModel = value;
                OnpropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDtlViewEventArgs args)
        {
            #region olds
            //Com tabs n se aplica porque mudamos o tab mas o outro continua aberto
            //if(SelectedDetailViewModel != null && SelectedDetailViewModel.HasChanges)
            //{
            //    var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
            //    if(result == MessageDialogResult.Cancel)
            //    {
            //        return;
            //    }
            //}

            //switch (args.ViewModelName)
            //{
            //    case nameof(SupplierDetailViewModel):
            //        DetailViewModel = _supplierDetailViewModelCreator();
            //        break;
            //    case nameof(MeetingDetailViewModel):
            //        DetailViewModel = _meetingDetailViewModelCreator();
            //        break;
            //    default:
            //        throw new Exception($"ViewModel {args.ViewModelName} not mapped");
            //}
            #endregion

            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id
            && vm.GetType().Name == args.ViewModelName);

            if(detailViewModel == null)
            {
                //Não existe na lista, novo tab
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                //Adicionado à lista de tabs
                DetailViewModels.Add(detailViewModel);
            }
            SelectedDetailViewModel = detailViewModel;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDtlViewEventArgs { ViewModelName = viewModelType.Name });
        }


        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            //apanhar o tab para remover
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id
           && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }

        }

    }

}
