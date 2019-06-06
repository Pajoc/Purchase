using Purchase.DataAccess;
using Purchase.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Repositories
{

    public class SupplierRepository : GenericRepository<Supplier,PurchaseDbContext>, ISupplierRepository
    {
        #region consultaAntigo
        //private PurchaseDbContext _context;

        //public SupplierRepository(PurchaseDbContext context)
        //{
        //    _context = context;
        //}

        //public void Add(Supplier supplier)
        //{
        //    _context.Suppliers.Add(supplier);
        //}

        //public void Remove(Supplier supplier)
        //{
        //    _context.Suppliers.Remove(supplier);
        //}

        //public async Task<Supplier> GetByIdAsync(int ID)
        //{
        //    //yield return new Supplier { Name = "Irmãos Valente", Code = "IRV" };
        //    //yield return new Supplier { Name = "Acebron", Code = "ACE" };
        //    //yield return new Supplier { Name = "Mecânica exata", Code = "EXA" };
        //    //yield return new Supplier { Name = "Fedex", Code = "FEX" };
        //    //yield return new Supplier { Name = "Embal segur", Code = "EBS" };

        //    return await _context.Suppliers
        //        .Include(f => f.PhoneNumbers)
        //        .SingleAsync(s => s.Id == ID);

        //}

        //public bool HasChanges()
        //{
        //    return _context.ChangeTracker.HasChanges();
        //}

        //public async Task SaveAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}

        //public void RemovePhoneNumber(SupplierPhoneNumber model)
        //{
        //    _context.SupplierPhoneNumbers.Remove(model);
        //}
        #endregion

        
        public SupplierRepository(PurchaseDbContext context):base (context)
        {
            
        }
        
        public override async Task<Supplier> GetByIdAsync(int ID)
        {
           
            return await Context.Suppliers
                .Include(f => f.PhoneNumbers)
                .SingleAsync(s => s.Id == ID);
        }

        public void RemovePhoneNumber(SupplierPhoneNumber model)
        {
            Context.SupplierPhoneNumbers.Remove(model);
        }


    }
}
