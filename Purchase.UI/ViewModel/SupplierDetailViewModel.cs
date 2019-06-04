using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Data.Repositories;
using Purchase.UI.Event;
using Purchase.UI.View.Services;
using Purchase.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class SupplierDetailViewModel : ViewModelBase, ISupplierDetailViewModel
    {
        private ISupplierRepository _supplierRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private SupplierWrapper _supplier;
        private bool _hasChanges;

        public SupplierDetailViewModel(ISupplierRepository supplierRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _supplierRepository = supplierRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
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

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnpropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public async Task LoadAsync(int? SupID)
        {
            var sup = SupID.HasValue
              ? await _supplierRepository.GetByIdAsync(SupID.Value)
              : CreateNewSupplier();

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

               
            };

            //Faz com que volte a verificar OnSaveCanExecute
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Supplier.Id == 0)
            {
                Supplier.Name = "";
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        private async void OnSaveExecute()
        {
            await _supplierRepository.SaveAsync();
            HasChanges = _supplierRepository.HasChanges();
            _eventAggregator.GetEvent<AfterSupplierSavedEvent>().Publish(
                new AfterSupplierSavedEventArgs
                {
                    Id = _supplier.Id,
                    DisplayMember = _supplier.Name
                }
           );
        }

        private bool OnSaveCanExecute()
        {
            return Supplier!=null && !Supplier.HasErrors && HasChanges;
        }
        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete this supplier {Supplier.Name}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _supplierRepository.Remove(Supplier.Model);
                await _supplierRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterSupplierDeletedEvent>().Publish(Supplier.Id);
            }
        }


        private Supplier CreateNewSupplier()
        {
            var supplier = new Supplier();

            _supplierRepository.Add(supplier);
            return supplier;
        }

    }
}
