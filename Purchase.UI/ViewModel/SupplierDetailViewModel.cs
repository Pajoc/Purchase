using Purchase.Model;
using Purchase.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class SupplierDetailViewModel : ViewModelBase, ISupplierDetailViewModel
    {
        private ISupplierDataService _supplierDataService;
        private Supplier _supplier;

        public SupplierDetailViewModel(ISupplierDataService supplierDataService)
        {
            _supplierDataService = supplierDataService;
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

        public async Task LoadAsync(int SupID)
        {
            Supplier = await _supplierDataService.GetByIdAsync(SupID);
        }
    }
}
