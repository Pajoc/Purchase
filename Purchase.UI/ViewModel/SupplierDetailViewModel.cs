using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Data.Loockups;
using Purchase.UI.Data.Repositories;
using Purchase.UI.Event;
using Purchase.UI.View.Services;
using Purchase.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class SupplierDetailViewModel : DetailViewModelBase, ISupplierDetailViewModel
    {
        private ISupplierRepository _supplierRepository;
        
        private ISupplierTypeLookupDataService _supplierTypeLookupDataService;
        private SupplierWrapper _supplier;
       
        private SupplierPhoneNumberWrapper _selectedPhoneNumber;

        public SupplierDetailViewModel(ISupplierRepository supplierRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService, ISupplierTypeLookupDataService supplierTypeLookupDataService):base(eventAggregator, messageDialogService)
        {
            _supplierRepository = supplierRepository;
            _supplierTypeLookupDataService = supplierTypeLookupDataService;

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            SupplierTypes = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<SupplierPhoneNumberWrapper>();

        }

        
        public SupplierWrapper Supplier {
            //Ao ativar o LoadAsync notifica a interface 
            get { return _supplier;  }
            private set
            {
                _supplier = value;
                OnpropertyChanged();
            }
        }


        public SupplierPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }

            set
            {
                _selectedPhoneNumber = value;
                OnpropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }

        }

        public override async Task LoadAsync(int SupID)
        {
            //var sup = SupID.HasValue ? await _supplierRepository.GetByIdAsync(SupID.Value) : : CreateNewSupplier();

            var sup = SupID > 0
              ? await _supplierRepository.GetByIdAsync(SupID)
              : CreateNewSupplier();

            Id = SupID;

            InitializeSupplier(sup);

            InitializeSupplierPhoneNumbers(sup.PhoneNumbers);

            await LoadSupplierTypesLookupAsync();
        }

        public ICommand AddPhoneNumberCommand { get; }

        public ICommand RemovePhoneNumberCommand { get; }

        public ObservableCollection<LookupItem> SupplierTypes { get; }
        public ObservableCollection<SupplierPhoneNumberWrapper> PhoneNumbers { get; }


        private void InitializeSupplier(Supplier sup)
        {
            Supplier = new SupplierWrapper(sup);

            Supplier.PropertyChanged += (s, e) =>
            {

                if (!HasChanges)
                {
                    HasChanges = _supplierRepository.HasChanges();
                }

                //o HasErrors tem de ativar PropertyChanged ir ao NotifyDataErrorInfoBase->OnErrorsChanged
                if (e.PropertyName == nameof(Supplier.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Supplier.Name))
                {
                    SetTitle();
                }

            };

            //Faz com que volte a verificar OnSaveCanExecute
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Supplier.Id == 0)
            {
                Supplier.Name = "";
            }

            SetTitle();

        }

        private void SetTitle()
        {
            Title = $"{Supplier.Code}";
        }

        private void InitializeSupplierPhoneNumbers(ICollection<SupplierPhoneNumber> phoneNumbers)
        {
         
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= SupplierPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var supplierPhoneNumber in phoneNumbers)
            {
                var wrapper = new SupplierPhoneNumberWrapper(supplierPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += SupplierPhoneNumberWrapper_PropertyChanged;
            }

        }

        private void SupplierPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _supplierRepository.HasChanges();
            }
            if (e.PropertyName == nameof(SupplierPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadSupplierTypesLookupAsync()
        {
            SupplierTypes.Clear();
            SupplierTypes.Add(new NullLookupItem { DisplayMember = " - " } );
            var lookup = await _supplierTypeLookupDataService.GetSupplierTypeLookupAsync();
            foreach (var lookupItem in lookup)
            {
                SupplierTypes.Add(lookupItem);
            }
        }

        protected override async void OnSaveExecute()
        {
            await _supplierRepository.SaveAsync();
            HasChanges = _supplierRepository.HasChanges();
            Id = Supplier.Id;

            RaiseDetailSavedEvent(Supplier.Id, Supplier.Name);
           // EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
           //     new AfterDetailSavedEventArgs
           //     {
           //         Id = _supplier.Id,
           //         DisplayMember = _supplier.Name,
           //         ViewModelName = nameof(SupplierDetailViewModel)
           //     }
           //);
        }

        protected override bool OnSaveCanExecute()
        {
            return Supplier!=null && !Supplier.HasErrors && PhoneNumbers.All(pn => !pn.HasErrors) && HasChanges;
        }
        protected override async void OnDeleteExecute()
        {

            if (await _supplierRepository.HasMeetingsAsync(Supplier.Id))
            {
                MessageDialogService.ShowInfoDialog($"{Supplier.Name} can't be deleted while he is part exists of a meeting.");
                return;
            }


            var result = MessageDialogService.ShowOkCancelDialog($"Do you really want to delete this supplier {Supplier.Name}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _supplierRepository.Remove(Supplier.Model);
                await _supplierRepository.SaveAsync();
                RaiseDetailDeletedEvent(Supplier.Id);
                //EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new AfterDetailDeletedEventArgs
                //{
                //    Id = Supplier.Id,
                //    ViewModelName = nameof(SupplierDetailViewModel)
                //});
            }
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new SupplierPhoneNumberWrapper(new SupplierPhoneNumber());
            newNumber.PropertyChanged += SupplierPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Supplier.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= SupplierPhoneNumberWrapper_PropertyChanged;
            //Supplier.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model); não funciona porque o id em phonenumbers não é nulo (nem deve ser é para remover tudo)
            _supplierRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _supplierRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private Supplier CreateNewSupplier()
        {
            var supplier = new Supplier();

            _supplierRepository.Add(supplier);
            return supplier;
        }

    }
}
