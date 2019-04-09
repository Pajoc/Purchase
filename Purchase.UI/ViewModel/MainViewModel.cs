using Purchase.Model;
using Purchase.UI.Data;
using System.Collections.ObjectModel;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private ISupplierDataService _supplierDataService;
        private Supplier _selectedSupplier;

        public MainViewModel(ISupplierDataService supplierDataService )
        {
            Suppliers = new ObservableCollection<Supplier>();
            _supplierDataService = supplierDataService;
        }

        //usado para notificar o DBinding k a collection mudou
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public void Load()
        {
            var suppliers = _supplierDataService.GetAll();
            Suppliers.Clear();
            foreach (var supplier in suppliers)
            {
                Suppliers.Add(supplier);
            }
        }

        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set {
                _selectedSupplier = value;
                //OnpropertyChanged("SelectedSupplier");
                OnpropertyChanged();
            }
        }

    }

    
}
