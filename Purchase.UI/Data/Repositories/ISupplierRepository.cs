﻿using System.Threading.Tasks;
using Purchase.Model;

namespace Purchase.UI.Data.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetByIdAsync(int ID);
        Task SaveAsync();
    }
}