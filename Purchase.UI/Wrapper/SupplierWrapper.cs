using Purchase.Model;
using System;

namespace Purchase.UI.Wrapper
{

    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model)
        {
            Model = model;
        }

        public T Model { get; }
    }

    public class SupplierWrapper : ModelWrapper<Supplier>
    {
        public SupplierWrapper(Supplier model) : base(model)
        {

        }

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
                    if (string.Equals(Name, "Beaudrey", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "Can't add self company");
                    }
                    break;

            }
        }

    }

}
