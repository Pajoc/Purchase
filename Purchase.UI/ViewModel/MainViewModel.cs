using Purchase.Model;
using Purchase.UI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Purchase.UI.ViewModel
{
    public class MainViewModel: INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set {
                _selectedSupplier = value;
                //OnpropertyChanged("SelectedSupplier");
                OnpropertyChanged();
            }
        }

        private void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
