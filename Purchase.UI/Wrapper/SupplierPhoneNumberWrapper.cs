using Purchase.Model;

namespace Purchase.UI.Wrapper
{
    public class SupplierPhoneNumberWrapper : ModelWrapper<SupplierPhoneNumber>
    {
        public SupplierPhoneNumberWrapper(SupplierPhoneNumber model) : base(model)
        {

        }

        public string Number
        {
            get
            {
                return GetValue<string>();
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
