using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;

namespace hrmApp.Data.Repositories
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Assignment> Get___ByIdAsync(int assignmentId)
        {
            var assignment = applicationDbContext.Assignments
                            .Include(x => x.ApplicationUser)
                            .Include(x => x.Organization)
                            .SingleOrDefaultAsync(x => x.Id == assignmentId);
            return await assignment;
        }
    }
}
