using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task<Organization> GetWithPOByIdAsync(int organizationId);
        Task<Organization> GetWithAssignmentsByIdAsync(int organizationId);
        Task<Organization> GetWithAllByIdAsync(int organizationId);
        Task<IEnumerable<Organization>> GetRelatedIdsByProjectIdAsync(int projectId);
        Task<IEnumerable<Organization>> GetByUserIdandProjectIdAsync(string applicationUserId, int projectId);
        Task<Organization> GetByEmployeeIdAsync(int employeeId);
    }
}