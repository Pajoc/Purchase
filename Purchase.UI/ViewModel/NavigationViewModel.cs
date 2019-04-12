using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
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

        private LookupSupplier _selectedSupplier;

        public LookupSupplier SelectedSupplier
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
