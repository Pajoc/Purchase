using Purchase.DataAccess;
using Purchase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private PurchaseDbContext _context;

        public SupplierRepository(PurchaseDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier> GetByIdAsync(int ID)
        {
            //yield return new Supplier { Name = "Irmãos Valente", Code = "IRV" };
            //yield return new Supplier { Name = "Acebron", Code = "ACE" };
            //yield return new Supplier { Name = "Mecânica exata", Code = "EXA" };
            //yield return new Supplier { Name = "Fedex", Code = "FEX" };
            //yield return new Supplier { Name = "Embal segur", Code = "EBS" };

            return await _context.Suppliers.SingleAsync(s => s.Id == ID);

        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
