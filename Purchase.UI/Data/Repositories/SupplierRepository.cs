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

        public void Add(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
        }

        public void Remove(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
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

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
