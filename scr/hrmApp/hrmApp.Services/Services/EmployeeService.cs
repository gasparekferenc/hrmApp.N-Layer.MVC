using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class EmployeeService : Service<Employee>, IEmployeeService
    {
        public EmployeeService(IUnitOfWork unitOfWork, IRepository<Employee> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<Employee> Get___ByIdAsync(int employeeId)
        {
            return await _unitOfWork.Employees.Get___ByIdAsync(employeeId);
        }
        public async Task<Employee> GetWithIncludesByIdAsync(int employeeId)
        {
            return await _unitOfWork.Employees.GetWithIncludesByIdAsync(employeeId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByUserIdAndProjectIdAysnc(string userId, int currentProjectId)
        {
            return await _unitOfWork.Employees.GetEmployeesByUserIdAndProjectIdAysnc(userId, currentProjectId);
        }
        public async Task<IEnumerable<Employee>> GetEmployeesByProjectIdAysnc(int currentProjectId)
        {
            return await _unitOfWork.Employees.GetEmployeesByProjectIdAysnc(currentProjectId);
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAysnc()
        {
            return await _unitOfWork.Employees.GetAllEmployeesAysnc();
        }


        public async Task<bool> ExistSSNumber(string sSNumber)
        {
            return await _unitOfWork.Employees.ExistSSNumber(sSNumber);
        }
        public async Task<Employee> GetBySSNumber(string sSNumber)
        {
            return await _unitOfWork.Employees.GetBySSNumber(sSNumber);
        }
        public async Task ActivateEmployeeAsync(Employee employee)
        {
            await _unitOfWork.Employees.ActivateEmployeeAsync(employee);
        }

        // public async Task AttachToProject(Employee employee, int projectId, int organizationId, string applicationUserId)
        // {
        //     await _unitOfWork.Employees.AttachToProject(employee, projectId, organizationId, applicationUserId);
        // }
    }
}

