using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? ID);
        bool HasChanges { get; }
        int Id { get; }
    }
}
