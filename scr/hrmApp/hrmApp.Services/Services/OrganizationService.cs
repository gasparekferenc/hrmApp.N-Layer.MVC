using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class OrganizationService : Service<Organization>, IOrganizationService
    {
        public OrganizationService(IUnitOfWork unitOfWork, IRepository<Organization> repository)
                                : base(unitOfWork, repository)
        {
        }
        public async Task<Organization> GetWithPOByIdAsync(int organizationId)
        {
            return await _unitOfWork.Organizations.GetWithPOByIdAsync(organizationId);
        }
        public async Task<Organization> GetWithAssignmentsByIdAsync(int organizationId)
        {
            return await _unitOfWork.Organizations.GetWithAssignmentsByIdAsync(organizationId);
        }
        public async Task<Organization> GetWithAllByIdAsync(int organizationId)
        {
            return await _unitOfWork.Organizations.GetWithAllByIdAsync(organizationId);
        }
        public async Task<IEnumerable<Organization>> GetRelatedIdsByProjectIdAsync(int projectId)
        {
            return await _unitOfWork.Organizations.GetRelatedIdsByProjectIdAsync(projectId);
        }
        public async Task<IEnumerable<Organization>> GetByUserIdandProjectIdAsync(string applicationUserId, int projectId)
        {
            return await _unitOfWork.Organizations.GetByUserIdandProjectIdAsync(applicationUserId, projectId);
        }
        public async Task<Organization> GetByEmployeeIdAsync(int employeeId)
        {
            return await _unitOfWork.Organizations.GetByEmployeeIdAsync(employeeId);
        }
    }
}