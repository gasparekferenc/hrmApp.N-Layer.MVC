using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        // Amennyiben szükség van új funkciókra itt definiálhatjuk
        Task<Assignment> Get___ByIdAsync(int assignmentId);
    }
}
