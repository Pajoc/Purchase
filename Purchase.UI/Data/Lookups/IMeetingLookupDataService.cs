using Purchase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Purchase.UI.Data.Loockups
{
    public interface IMeetingLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync();
    }
}