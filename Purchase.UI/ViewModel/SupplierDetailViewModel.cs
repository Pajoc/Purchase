using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Event;
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
        private ISupplierDataService _supplierDataService;
        private IEventAggregator _eventAggregator;
        private Supplier _supplier;

        public SupplierDetailViewModel(ISupplierDataService supplierDataService, IEventAggregator eventAggregator)
        {
            _supplierDataService = supplierDataService;
            _eventAggregator = eventAggregator;
           
           _eventAggregator.GetEvent<OpenSupplierDtlViewEvent>().Subscribe(OnOpenSupplierDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        

        private async void OnOpenSupplierDetailView(int SupplierId)
        {
            await LoadAsync(SupplierId);
        }

        public Supplier Supplier {
            //Ao ativar o LoadAsync notifica a interface 
            get { return _supplier;  }
            private set
            {
                _supplier = value;
                OnpropertyChanged();
            }
        }

        private void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        private bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync(int SupID)
        {
            Supplier = await _supplierDataService.GetByIdAsync(SupID);
        }

        public ICommand SaveCommand { get; }

       
    }
}
