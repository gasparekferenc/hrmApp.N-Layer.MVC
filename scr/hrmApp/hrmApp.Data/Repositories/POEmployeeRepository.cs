using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Linq;
using System.Collections.Generic;

namespace hrmApp.Data.Repositories
{
    public class POEmployeeRepository : Repository<POEmployee>, IPOEmployeeRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public POEmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<POEmployee> Get___ByIdAsync(int poEmployeeId)
        {
            var poEmployee = applicationDbContext.POEmployees
                            .Include(x => x.ProjectOrganization)
                            .Include(x => x.Employee)
                            .SingleOrDefaultAsync(x => x.Id == poEmployeeId);
            return await poEmployee;
        }
        public async Task<IEnumerable<POEmployee>> GetAllByProjectIdAsync(int projectId)
        {
            var poEmployee = await applicationDbContext.POEmployees
                            .Include(poe => poe.ProjectOrganization)
                                .ThenInclude(po => po.Organization)
                            .Where(x => x.ProjectOrganization.ProjectId == projectId)
                            .ToListAsync();
                            // .SingleOrDefaultAsync();
            return poEmployee;
        }
    }
}
