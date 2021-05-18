using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IAssignmentService : IService<Assignment>
    {
        Task<Assignment> Get___ByIdAsync(int assignmentId);
    }
}
