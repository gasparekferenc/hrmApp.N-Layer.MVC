using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IProjectOrganizationService : IService<ProjectOrganization>
    {
        Task<IEnumerable<ProjectOrganization>> GetAllWithIncludesAsync();
        Task<IEnumerable<ProjectOrganization>> GetAllByProjectIdAsync(int projectId);
        Task<ProjectOrganization> Get___ByIdAsync(int projectOrganizationId);
        Task<ProjectOrganization> GetByProjectIdAndOrganizationIdAsync(int projectId, int organizationId);

    }
}
