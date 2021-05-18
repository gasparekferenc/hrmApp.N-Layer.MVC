using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;

namespace hrmApp.Data.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public OrganizationRepository(ApplicationDbContext context) : base(context)
        {
        }

        #region GetWithPOByIdAsync(int organizationId)
        public async Task<Organization> GetWithPOByIdAsync(int organizationId)
        {
            var organization = applicationDbContext.Organizations
                            .Include(x => x.ProjectOrganizations)
                            .Include(x => x.Assignments)
                            .SingleOrDefaultAsync(x => x.Id == organizationId);
            return await organization;
        }
        #endregion

        #region GetWithAssignmentsByIdAsync(int organizationId)            

        public async Task<Organization> GetWithAssignmentsByIdAsync(int organizationId)
        {
            var organization = applicationDbContext.Organizations
                            .Include(x => x.Assignments)
                            .SingleOrDefaultAsync(x => x.Id == organizationId);
            return await organization;
        }

        #endregion

        #region GetWithAllByIdAsync(int organizationId)

        public async Task<Organization> GetWithAllByIdAsync(int organizationId)
        {
            var organization = applicationDbContext.Organizations
                            .Include(x => x.ProjectOrganizations)
                            .Include(x => x.Assignments)
                            .SingleOrDefaultAsync(x => x.Id == organizationId);
            return await organization;
        }

        #endregion

        #region GetRelatedIdsByProjectIdAsync(int projectId)

        public async Task<IEnumerable<Organization>> GetRelatedIdsByProjectIdAsync(int projectId)
        {
            var organizationIds = (from o in applicationDbContext.Organizations
                                   join po in applicationDbContext.ProjectOrganizations
                                           on o.Id equals po.OrganizationId
                                   join poe in applicationDbContext.POEmployees
                                           on po.Id equals poe.ProjectOrganizationId
                                   where po.ProjectId == projectId
                                   select o)
                                    .ToListAsync();

            return await organizationIds;
        }
        #endregion

        #region GetByUserIdandProjectIdAsync(string applicationUserId, int projectId)

        public async Task<IEnumerable<Organization>> GetByUserIdandProjectIdAsync(string applicationUserId, int projectId)
        {
            var organizations = (from o in applicationDbContext.Organizations
                                 join a in applicationDbContext.Assignments
                                         on o.Id equals a.OrganizationId
                                 join po in applicationDbContext.ProjectOrganizations
                                         on o.Id equals po.OrganizationId
                                 where a.ApplicationUserId == applicationUserId
                                         && po.ProjectId == projectId
                                 select o)
                                .Include(o => o.ProjectOrganizations)
                                .OrderBy(o => o.OrganizationName)
                                .ToListAsync();

            return await organizations;
        }
        #endregion

        #region GetByEmployeeIdAsync(int employeeId)

        public async Task<Organization> GetByEmployeeIdAsync(int employeeId)
        {
            var organization = (from o in applicationDbContext.Organizations
                                join po in applicationDbContext.ProjectOrganizations
                                        on o.Id equals po.OrganizationId
                                join poe in applicationDbContext.POEmployees
                                        on po.Id equals poe.ProjectOrganizationId
                                where poe.EmployeeId == employeeId
                                select o)
                                .SingleOrDefaultAsync();
            return await organization;
        }
        #endregion

        #region public async Task<IEnumerable<int>> GetRelatedIdsByProjectIdAsync2(int projectId)

        // FluentAPI verzió
        // public async Task<IEnumerable<int>> GetRelatedIdsByProjectIdAsync2(int projectId)
        // {
        //     var organizationIds = applicationDbContext.Organizations
        //                             .Join(applicationDbContext.ProjectOrganizations,
        //                                 o => o.Id,
        //                                 po => po.OrganizationId,
        //                                 (o, po) => new
        //                                 {
        //                                     organizationId = o.Id,
        //                                     projectOrganizationId = po.Id,
        //                                     projectId = po.ProjectId
        //                                 }
        //                             )
        //                             .Join(applicationDbContext.POEmployees,
        //                                 po => po.projectOrganizationId,
        //                                 poe => poe.ProjectOrganizationId,
        //                                 (po, poe) => new
        //                                 {
        //                                     organizationId = po.organizationId,
        //                                     projectId = po.projectId
        //                                 }
        //                             )
        //                             .Where(p => p.projectId == projectId)
        //                             .Select(o => o.organizationId)
        //                             .ToListAsync();

        //     return await organizationIds;
        // }

        #endregion

    }
}
