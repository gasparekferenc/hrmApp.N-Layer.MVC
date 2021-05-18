using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Collections.Generic;

namespace hrmApp.Data.Repositories
{
    public class ProjectOrganizationRepository : Repository<ProjectOrganization>, IProjectOrganizationRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public ProjectOrganizationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProjectOrganization>> GetAllWithIncludesAsync()
        {
            var projectOrganizations = await applicationDbContext.ProjectOrganizations
                            .Include(x => x.Project)
                            .Include(x => x.Organization)
                            .ToListAsync();
            return projectOrganizations;
        }
        public async Task<ProjectOrganization> Get___ByIdAsync(int projectOrganizationId)
        {
            var projectOrganization = applicationDbContext.ProjectOrganizations
                            .Include(x => x.Project)
                            .Include(x => x.Organization)
                            .SingleOrDefaultAsync(x => x.Id == projectOrganizationId);
            return await projectOrganization;
        }
        public async Task<ProjectOrganization> GetByProjectIdAndOrganizationIdAsync(int projectId, int organizationId)
        {
            var projectOrganization = applicationDbContext.ProjectOrganizations
                            .SingleOrDefaultAsync(x => x.ProjectId == projectId && x.OrganizationId == organizationId);
            return await projectOrganization;
        }
    }
}
