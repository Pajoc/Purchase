using Purchase.Model;
using Purchase.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Purchase.UI.Wrapper
{
    public class SupplierWrapper : ViewModelBase, INotifyDataErrorInfo
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
                ValidateProperty(nameof(Name));
            }
        }

        public string Code
        {
            get { return Model.Code; }
            set
            {
                Model.Code = value;
                OnpropertyChanged();
            }
        }

        public string MainEmail
        {
            get { return Model.MainEmail; }
            set
            {
                Model.MainEmail = value;
                OnpropertyChanged();
            }
        }

        private void ValidateProperty(string propertyName)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case nameof(Name):
                    if(string.Equals(Name,"Beaudrey",StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "Can't add self company");
                    }
                    break;
                   
            }
        }

       

        private Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        //public bool HasErrors()
        //{
        //    return _errorsByPropertyName.Any();
        //}

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
