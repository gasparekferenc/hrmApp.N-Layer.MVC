using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class POEmployeeService : Service<POEmployee>, IPOEmployeeService
    {
        public POEmployeeService(IUnitOfWork unitOfWork, IRepository<POEmployee> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<POEmployee> Get___ByIdAsync(int poEmployeeId)
        {
            return await _unitOfWork.POEmployees.Get___ByIdAsync(poEmployeeId);
        }
        public async Task<IEnumerable<POEmployee>> GetAllByProjectIdAsync(int projectId)
        {
            return await _unitOfWork.POEmployees.GetAllByProjectIdAsync(projectId);
        }
    }
}
