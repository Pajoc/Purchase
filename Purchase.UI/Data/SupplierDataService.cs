using Purchase.Model;
using System.Collections.Generic;

namespace Purchase.UI.Data
{
    public class SupplierDataService : ISupplierDataService
    {
        public IEnumerable<Supplier> GetAll()
        {
            yield return new Supplier { Name = "Irmãos Valente", Code = "IRV" };
            yield return new Supplier { Name = "Acebron", Code = "ACE" };
            yield return new Supplier { Name = "Mecânica exata", Code = "EXA" };
            yield return new Supplier { Name = "Fedex", Code = "FEX" };
            yield return new Supplier { Name = "Embal segur", Code = "EBS" };
        }
    }
}
