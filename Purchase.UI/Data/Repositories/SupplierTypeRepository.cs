using Purchase.DataAccess;
using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Repositories
{
    public class SupplierTypeRepository: GenericRepository<SupplierType, PurchaseDbContext>, ISupplierTypeRepository
    {
        public SupplierTypeRepository(PurchaseDbContext context): base (context)
        {

        }

        public async Task<bool> IsReferencedBySupplierAsync(int supplierTypeId)
        {
            return await Context.Suppliers.AsNoTracking()
                .AnyAsync(s => s.TypeOfSupplierId == supplierTypeId);
        }
    }
}
