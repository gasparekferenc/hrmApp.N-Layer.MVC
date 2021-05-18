using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IPOEmployeeRepository : IRepository<POEmployee>
    {
        // just a template
        Task<POEmployee> Get___ByIdAsync(int poEmployeeId);
        Task<IEnumerable<POEmployee>> GetAllByProjectIdAsync(int projectId);
    }
}
