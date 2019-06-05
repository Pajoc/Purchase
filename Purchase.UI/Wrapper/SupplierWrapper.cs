using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Purchase.UI.Wrapper
{

    public class SupplierWrapper : ModelWrapper<Supplier>
    {
        public SupplierWrapper(Supplier model) : base(model)
        {

        }

        public int Id { get { return GetValue<int>(); } }

        public string Name
        {
            //1º get { return Model.Name; }
            //2º get { return GetValue<string>(nameof(Name)); }
            get { return GetValue<string>(); }

            set
            {
                //Model.Name = value;
                SetValue(value);
                //ValidateProperty(nameof(Name));
            }
        }

       

        public string Code
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string MainEmail
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public int? TypeOfSupplierId {
            get
            {
                return GetValue<int?>();
            }
            set
            {
                SetValue(value);
            }
        }


        //private void ValidateProperty(string propertyName)
        //{
        //    ClearErrors(propertyName);
        //    switch (propertyName)
        //    {
        //        case nameof(Name):
        //            if (string.Equals(Name, "Beaudrey", StringComparison.OrdinalIgnoreCase))
        //            {
        //                AddError(propertyName, "Can't add self company");
        //            }
        //            break;

        //    }
        //}

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (string.Equals(Name, "Beaudrey", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Can't add self company";
                    }
                    break;
                case nameof(Code):
                    if (string.Equals(Code, "BAS", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Can't add delegations";
                    }
                    break;
            }
        }
    }

}
