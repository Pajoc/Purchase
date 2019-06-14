using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.Wrapper
{
    public class SupplierTypeWrapper : ModelWrapper<SupplierType>
    {
        public SupplierTypeWrapper(SupplierType model) : base(model)
        {

        }
        public int Id { get { return Model.Id; } }

        public string Type
        {
            get { return GetValue<string> (); }

            set { SetValue(value); }
        }
    }
}
