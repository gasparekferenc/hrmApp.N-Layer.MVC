using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IPOEmployeeService : IService<POEmployee>
    {
        Task<POEmployee> Get___ByIdAsync(int poEmployeeId);
        Task<IEnumerable<POEmployee>> GetAllByProjectIdAsync(int projectId);
    }
}
