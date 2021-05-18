using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IProjectOrganizationRepository : IRepository<ProjectOrganization>
    {
        Task<IEnumerable<ProjectOrganization>> GetAllWithIncludesAsync();
        Task<ProjectOrganization> Get___ByIdAsync(int projectOrganizationId);
        Task<ProjectOrganization> GetByProjectIdAndOrganizationIdAsync(int projectId, int organizationId);
    }
}
