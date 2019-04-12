using Purchase.Model;
using Purchase.UI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private ILookupSupplierDataService _supplierLookUpDServ;

        public NavigationViewModel(ILookupSupplierDataService supplierLookUpDServ)
        {
            _supplierLookUpDServ = supplierLookUpDServ;
            Suppliers = new ObservableCollection<LookupSupplier>();
        }

        public async Task LoadAsync()
        {
            var lookUp = await _supplierLookUpDServ.GetSupplierLookupAsync();
            Suppliers.Clear();
            foreach (var supplier in lookUp)
            {
                Suppliers.Add(supplier);
            }
        }

        public ObservableCollection<LookupSupplier> Suppliers { get; }

    }
}
