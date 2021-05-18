using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace hrmApp.Data.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetAllWithIncludesAsync()
        {
            var projects = await applicationDbContext.Projects
                            .Include(p => p.ProjectOrganizations)
                                .ThenInclude(po => po.Organization)
                            .OrderBy(p => p.ProjectName)
                            // .OrderByDescending (p => p.StartDate)
                            // .Where(p => p.IsActive)
                            .AsNoTracking()
                            .ToListAsync();
            return projects;
        }
        public async Task<Project> GetWithIncludesByIdAsync(int projectId)
        {
            var project = await applicationDbContext.Projects
                            .Include(p => p.ProjectOrganizations)
                                .ThenInclude(po => po.Organization)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(p => p.Id == projectId);
            return project;
        }
        public async Task<Project> GetWithPOByIdAsync(int projectId)
        {
            var project = await applicationDbContext.Projects
                            .Include(p => p.ProjectOrganizations)
                            .SingleOrDefaultAsync(p => p.Id == projectId);
            return project;
        }
    }
}
