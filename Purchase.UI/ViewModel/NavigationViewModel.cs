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
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookUp = await _supplierLookUpDServ.GetSupplierLookupAsync();
            Suppliers.Clear();
            foreach (var supplier in lookUp)
            {
                //Suppliers.Add(supplier);
                Suppliers.Add(new NavigationItemViewModel(supplier.Id,supplier.DisplayMember,nameof(SupplierDetailViewModel), _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Suppliers { get; }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {

            switch (args.ViewModelName)
            {
                case nameof(SupplierDetailViewModel):
                    var supplier = Suppliers.SingleOrDefault(s => s.Id == args.Id);

                    if (supplier != null)
                    {
                        Suppliers.Remove(supplier);
                    }
                    break;
            }

        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {

            switch (obj.ViewModelName)
            {
                case nameof(SupplierDetailViewModel):

                    var lookupItem = Suppliers.SingleOrDefault(l => l.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Suppliers.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, nameof(SupplierDetailViewModel), _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }
                    break;
            }
        }
    }
}
