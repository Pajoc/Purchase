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
        }

        private void AfterSupplierSaved(AfterSupplierSavedEventArgs supplier)
        {
            var lookupItem = Suppliers.Single(l => l.Id == supplier.Id);
            lookupItem.DisplayMember = supplier.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookUp = await _supplierLookUpDServ.GetSupplierLookupAsync();
            Suppliers.Clear();
            foreach (var supplier in lookUp)
            {
                //Suppliers.Add(supplier);
                Suppliers.Add(new NavigationItemViewModel(supplier.Id,supplier.DisplayMember));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Suppliers { get; }

        private NavigationItemViewModel _selectedSupplier;

        public NavigationItemViewModel SelectedSupplier
        {
            get { return _selectedSupplier; }
            set { _selectedSupplier = value;
                OnpropertyChanged();
                if (_selectedSupplier != null)
                {
                    _eventAggregator.GetEvent<OpenSupplierDtlViewEvent>().Publish(_selectedSupplier.Id);
                }
            }
        }


    }
}
