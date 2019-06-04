using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Data.Loockups;
using Purchase.UI.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private ILookupSupplierDataService _supplierLookUpDServ;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(ILookupSupplierDataService supplierLookUpDServ, IEventAggregator eventAggregator)
        {
            _supplierLookUpDServ = supplierLookUpDServ;
            _eventAggregator = eventAggregator;
            Suppliers = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterSupplierSavedEvent>().Subscribe(AfterSupplierSaved);
            eventAggregator.GetEvent<AfterSupplierDeletedEvent>().Subscribe(AfterSupplierDeleted);
        }

        public async Task LoadAsync()
        {
            var lookUp = await _supplierLookUpDServ.GetSupplierLookupAsync();
            Suppliers.Clear();
            foreach (var supplier in lookUp)
            {
                //Suppliers.Add(supplier);
                Suppliers.Add(new NavigationItemViewModel(supplier.Id,supplier.DisplayMember, _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Suppliers { get; }

        private void AfterSupplierDeleted(int supplierId)
        {
            var supplier = Suppliers.SingleOrDefault(s => s.Id == supplierId);

            if (supplier != null)
            {
                Suppliers.Remove(supplier);
            }
        }

        private void AfterSupplierSaved(AfterSupplierSavedEventArgs supplier)
        {
            var lookupItem = Suppliers.SingleOrDefault(l => l.Id == supplier.Id);
            if (lookupItem == null)
            {
                Suppliers.Add(new NavigationItemViewModel(supplier.Id, supplier.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = supplier.DisplayMember;
            }
        }

    }
}
