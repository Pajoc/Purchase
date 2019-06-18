using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data.Repositories;
using Purchase.UI.Event;
using Purchase.UI.View.Services;
using Purchase.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        //private IMessageDialogService _messageDialogService;
        private MeetingWrapper _meeting;
        private Supplier _selectedAvailableSupplier;
        private Supplier _selectedAddedSupplier;
        private List<Supplier> _allSuppliers;

        public MeetingDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator, messageDialogService)
        {
            _meetingRepository = meetingRepository;

            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddedSuppliers = new ObservableCollection<Supplier>();
            AvailableSuppliers = new ObservableCollection<Supplier>();
            AddSupplierCommand = new DelegateCommand(OnAddSupplierExecute, OnAddSupplierCanExecute);
            RemoveSupplierCommand = new DelegateCommand(OnRemoveSupplierExecute, OnRemoveSupplierCanExecute);

        }

       
        public MeetingWrapper Meeting
        {
            get { return _meeting;}
            private set
            {
                _meeting = value;
                OnpropertyChanged();
            }
        }


        public ICommand AddSupplierCommand { get; }

        public ICommand RemoveSupplierCommand { get; }

        public ObservableCollection<Supplier> AddedSuppliers { get; }

        public ObservableCollection<Supplier> AvailableSuppliers { get; }


        public Supplier SelectedAvailableSuppliers
        {
            get  { return _selectedAvailableSupplier; }

            set
            {
                _selectedAvailableSupplier = value;
                OnpropertyChanged();
                ((DelegateCommand)AddSupplierCommand).RaiseCanExecuteChanged();
            }
        }

        public Supplier SelectedAddedSuppliers
        {
            get { return _selectedAddedSupplier; }

            set
            {
                _selectedAddedSupplier = value;
                OnpropertyChanged();
                ((DelegateCommand)RemoveSupplierCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int meetingId)
        {
            var meeting = meetingId > 0 ? await _meetingRepository.GetByIdAsync(meetingId) : CreateNewMeeting();

            Id = meetingId;

            InitializeMeeting(meeting);

            //Load Sup for meetings
            _allSuppliers = await _meetingRepository.GetAllSuppliersAsync();

            SetupPicklist();
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof (Meeting.Title))
                {
                    SetTitle();
                }

            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if(Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        private void SetupPicklist()
        {
            var meetingSuppliersIds = Meeting.Model.Suppliers.Select(s => s.Id).ToList();
            var addedSuppliers = _allSuppliers.Where(f => meetingSuppliersIds.Contains(f.Id)).OrderBy(s => s.Name);
            var availableSuppliers = _allSuppliers.Except(addedSuppliers).OrderBy(s => s.Name);

            AddedSuppliers.Clear();
            AvailableSuppliers.Clear();

            foreach(var addedSup in addedSuppliers)
            {
                AddedSuppliers.Add(addedSup);
            }

            foreach (var avlSup in availableSuppliers)
            {
                AvailableSuppliers.Add(avlSup);
            }

        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected async override void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Do you really want to delete this meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _meetingRepository.Remove(Meeting.Model);
                await _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected async override void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }


        private bool OnRemoveSupplierCanExecute()
        {
            return SelectedAddedSuppliers != null;
        }

        private bool OnAddSupplierCanExecute()
        {
            return SelectedAvailableSuppliers != null;
        }

        private void OnRemoveSupplierExecute()
        {
            var supplierToRemove = SelectedAddedSuppliers;

            Meeting.Model.Suppliers.Remove(supplierToRemove);
            AddedSuppliers.Remove(supplierToRemove);
            AvailableSuppliers.Add(supplierToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddSupplierExecute()
        {
            var supplierToAdd = SelectedAvailableSuppliers;

            Meeting.Model.Suppliers.Add(supplierToAdd);
            AddedSuppliers.Add(supplierToAdd);
            AvailableSuppliers.Remove(supplierToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }


        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(SupplierDetailViewModel))
            {
                //força refresh
                await _meetingRepository.ReloadSupplierAsync(args.Id);
                _allSuppliers = await _meetingRepository.GetAllSuppliersAsync();
                SetupPicklist();
            }
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(SupplierDetailViewModel))
            {
                //como é delete não precisa de refresh
                _allSuppliers = await _meetingRepository.GetAllSuppliersAsync();
                SetupPicklist();
            }
        }

    }
}
