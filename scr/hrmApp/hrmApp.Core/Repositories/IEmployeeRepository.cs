using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> Get___ByIdAsync(int employeeId);
        Task<Employee> GetWithIncludesByIdAsync(int employeeId);
        Task<IEnumerable<Employee>> GetEmployeesByUserIdAndProjectIdAysnc(string userId, int currentProjectId);
        Task<IEnumerable<Employee>> GetEmployeesByProjectIdAysnc(int currentProjectId);
        Task<IEnumerable<Employee>> GetAllEmployeesAysnc();
        Task<bool> ExistSSNumber(string sSNumber);
        Task<Employee> GetBySSNumber(string sSNumber);
        Task ActivateEmployeeAsync(Employee employee);

        // Task AttachToProject(Employee employee, int projectId, int organizationId, string applicationUserId);
    }

}
