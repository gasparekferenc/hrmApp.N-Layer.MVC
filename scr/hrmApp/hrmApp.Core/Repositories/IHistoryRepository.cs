using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IHistoryRepository : IRepository<History>
    {
        Task<History> Get___ByIdAsync(int historyId);
        Task<IEnumerable<History>> GetAllByEmployeeIdAsync(int employeeId);
    }
}
