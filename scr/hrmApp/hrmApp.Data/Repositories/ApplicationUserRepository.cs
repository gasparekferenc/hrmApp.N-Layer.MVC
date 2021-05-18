using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace hrmApp.Data.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        #region Task<IEnumerable<ApplicationUser>> GetAllWithIncludesAsync()

        public async Task<IEnumerable<ApplicationUser>> GetAllWithIncludesAsync()
        {
            var applicationUsers = applicationDbContext.Users
                            .Include(u => u.Assignments)
                                .ThenInclude(a => a.Organization)
                            .OrderBy(u => u.UserName)
                            .AsNoTracking()
                            .ToListAsync();

            return await applicationUsers;
        }
        #endregion

        #region GetWithIncludesByIdAsync(string applicationUserId)

        public async Task<ApplicationUser> GetWithIncludesByIdAsync(string applicationUserId)
        {
            var applicationUser = await applicationDbContext.Users
                            .Include(u => u.Assignments)
                                .ThenInclude(a => a.Organization)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(u => u.Id == applicationUserId);
            return applicationUser;
        }
        #endregion

        #region GetWithAssignmentsByIdAsync(string applicationUserId)

        public async Task<ApplicationUser> GetWithAssignmentsByIdAsync(string applicationUserId)
        {
            var applicationUser = await applicationDbContext.Users
                            .Include(u => u.Assignments)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(u => u.Id == applicationUserId);
            return applicationUser;
        }
        #endregion

        #region GetProjectsOfUserAsync(string applicationUserId);
        public async Task<IEnumerable<Project>> GetProjectsOfUserAsync(string applicationUserId)
        {
            var userOfProjects = (from p in applicationDbContext.Projects
                                  join po in applicationDbContext.ProjectOrganizations
                                          on p.Id equals po.ProjectId
                                  join o in applicationDbContext.Organizations
                                          on po.OrganizationId equals o.Id
                                  join a in applicationDbContext.Assignments
                                          on o.Id equals a.OrganizationId
                                  where a.ApplicationUserId == applicationUserId
                                  select p)
                                    .Distinct()
                                    .OrderBy(p => p.EndDate)
                                    .ToListAsync();

            return await userOfProjects;
        }
        #endregion

        #region GetUserByEmailAsync(string email)

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var applicationUser = await applicationDbContext.Users
                            .AsNoTracking()
                            .SingleOrDefaultAsync(u => u.Email == email);
            return applicationUser;
        }
        #endregion

    }
}
