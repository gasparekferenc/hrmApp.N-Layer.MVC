using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace hrmApp.Services.Services
{
    public class ProjectOrganizationService : Service<ProjectOrganization>, IProjectOrganizationService
    {
        public ProjectOrganizationService(IUnitOfWork unitOfWork, IRepository<ProjectOrganization> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<IEnumerable<ProjectOrganization>> GetAllWithIncludesAsync()
        {
            return await _unitOfWork.ProjectOrganizations.GetAllWithIncludesAsync();
        }
        public async Task<IEnumerable<ProjectOrganization>> GetAllByProjectIdAsync(int projectId)
        {
            return (await _unitOfWork.ProjectOrganizations.GetAllAsync())
                                                          .Where(po => po.ProjectId == projectId);
        }

        public async Task<ProjectOrganization> Get___ByIdAsync(int projectOrganizationId)
        {
            return await _unitOfWork.ProjectOrganizations.Get___ByIdAsync(projectOrganizationId);
        }
        public async Task<ProjectOrganization> GetByProjectIdAndOrganizationIdAsync(int projectId, int organizationId)
        {
            return await _unitOfWork.ProjectOrganizations.GetByProjectIdAndOrganizationIdAsync(projectId, organizationId);
        }
    }
}
