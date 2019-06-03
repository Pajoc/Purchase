using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Data.Repositories;
using Purchase.UI.Event;
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
        private SupplierWrapper _supplier;
        private bool _hasChanges;

        public SupplierDetailViewModel(ISupplierRepository supplierRepository, IEventAggregator eventAggregator)
        {
            _supplierRepository = supplierRepository;
            _eventAggregator = eventAggregator;
           
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
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


        public async Task LoadAsync(int SupID)
        {

            var sup = await _supplierRepository.GetByIdAsync(SupID);

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
        }

        public ICommand SaveCommand { get; }

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

    }
}
