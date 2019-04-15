using Purchase.Model;
using Purchase.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Purchase.UI.Wrapper
{
    public class SupplierWrapper : ViewModelBase,INotifyDataErrorInfo
    {
        public SupplierWrapper(Supplier model)
        {
            Model = model;
        }

        public Supplier Model { get; }

        public int Id { get { return Model.Id; } }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                Model.Name = value;
                OnpropertyChanged();
            }
        }

        public string Email
        {
            get { return Model.MainEmail; }
            set
            {
                Model.MainEmail = value;
                OnpropertyChanged();
            }
        }

        private Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();
        

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
