using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;

namespace hrmApp.Services.Services
{
    public class AssignmentService : Service<Assignment>, IAssignmentService
    {
        public AssignmentService(IUnitOfWork unitOfWork, IRepository<Assignment> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<Assignment> Get___ByIdAsync(int assignmentId)
        {
            return await _unitOfWork.Assignments.Get___ByIdAsync(assignmentId);
        }
    }
}
