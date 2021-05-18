using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace hrmApp.Data.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        #region GetWithIncludesByIdAsync(int employeeId)

        public async Task<Employee> GetWithIncludesByIdAsync(int employeeId)
        {
            var employee = applicationDbContext.Employees
                            .Include(x => x.POEmployee)
                            .Include(x => x.ProcessStatus)
                            .Include(x => x.Job)
                            .Include(x => x.Documents)
                            .Include(x => x.Histories)
                            .SingleOrDefaultAsync(x => x.Id == employeeId);
            return await employee;
        }
        #endregion

        #region Get___ByIdAsync(int employeeId)

        public async Task<Employee> Get___ByIdAsync(int employeeId)
        {
            var employee = applicationDbContext.Employees
                            .Include(x => x.POEmployee)
                            .Include(x => x.ProcessStatus)
                            .Include(x => x.Job)
                            .Include(x => x.Documents)
                            .Include(x => x.Histories)
                            .SingleOrDefaultAsync(x => x.Id == employeeId);
            return await employee;
        }
        #endregion

        #region GetEmployeesByUserIdAndProjectIdAysnc(string userId, int currentProjectId)

        public async Task<IEnumerable<Employee>> GetEmployeesByUserIdAndProjectIdAysnc(string applicationUserId, int currentProjectId)
        {
            var employees = (from e in applicationDbContext.Employees
                             join poe in applicationDbContext.POEmployees
                                     on e.Id equals poe.EmployeeId
                             join po in applicationDbContext.ProjectOrganizations
                                     on poe.ProjectOrganizationId equals po.Id
                             join o in applicationDbContext.Organizations
                                     on po.OrganizationId equals o.Id
                             join a in applicationDbContext.Assignments
                                     on o.Id equals a.OrganizationId
                             where a.ApplicationUserId == applicationUserId
                                       && po.ProjectId == currentProjectId
                             select (e))
                            .Include(e => e.POEmployee)
                                .ThenInclude(poe => poe.ProjectOrganization)
                                    .ThenInclude(po => po.Organization)
                            .Include(e => e.ProcessStatus)
                            .Include(e => e.Job)
                            .ToListAsync();
            return await employees;
        }
        #endregion

        #region GetEmployeesByProjectIdAysnc(int currentProjectId)

        public async Task<IEnumerable<Employee>> GetEmployeesByProjectIdAysnc(int currentProjectId)
        {
            var employees = (from e in applicationDbContext.Employees
                             join poe in applicationDbContext.POEmployees
                                     on e.Id equals poe.EmployeeId
                             join po in applicationDbContext.ProjectOrganizations
                                     on poe.ProjectOrganizationId equals po.Id
                             join o in applicationDbContext.Organizations
                                     on po.OrganizationId equals o.Id
                             // nem biztos, hogy kell...
                             join a in applicationDbContext.Assignments
                                     on o.Id equals a.OrganizationId
                             join u in applicationDbContext.Users
                                     on a.ApplicationUserId equals u.Id
                             // ...eddig
                             where po.ProjectId == currentProjectId
                             select (e))
                            .Include(e => e.POEmployee)
                                .ThenInclude(poe => poe.ProjectOrganization)
                                    .ThenInclude(po => po.Organization)
                                        .ThenInclude(u => u.Assignments)
                            .Include(e => e.POEmployee)
                                .ThenInclude(poe => poe.ProjectOrganization)
                                    .ThenInclude(po => po.Project)
                            .Include(e => e.ProcessStatus)
                            .Include(e => e.Job)
                            .Distinct()
                            .ToListAsync();
            return await employees;
        }
        #endregion        

        #region GetAllEmployeesAysnc()

        public async Task<IEnumerable<Employee>> GetAllEmployeesAysnc()
        {
            var employees = (from e in applicationDbContext.Employees
                             join poe in applicationDbContext.POEmployees
                                     on e.Id equals poe.EmployeeId
                             join po in applicationDbContext.ProjectOrganizations
                                     on poe.ProjectOrganizationId equals po.Id
                             join p in applicationDbContext.Projects
                                     on po.ProjectId equals p.Id
                             join o in applicationDbContext.Organizations
                                     on po.OrganizationId equals o.Id
                             // nem biztos, hogy kell...
                             join a in applicationDbContext.Assignments
                                     on o.Id equals a.OrganizationId
                             join u in applicationDbContext.Users
                                     on a.ApplicationUserId equals u.Id
                             // ...eddig                             
                             select (e))
                            .Include(e => e.POEmployee)
                                .ThenInclude(poe => poe.ProjectOrganization)
                                    .ThenInclude(po => po.Organization)
                                        .ThenInclude(u => u.Assignments)
                            .Include(e => e.POEmployee)
                                .ThenInclude(poe => poe.ProjectOrganization)
                                    .ThenInclude(po => po.Project)
                            .Include(e => e.ProcessStatus)
                            .Include(e => e.Job)
                            .Distinct()
                            .ToListAsync();
            return await employees;
        }
        #endregion


        #region ExistSSNumber(string sSNumber)
        public async Task<bool> ExistSSNumber(string sSNumber)
        {
            var isExist = applicationDbContext.Employees
                .AnyAsync(e => e.SSNumber == sSNumber);
            return await isExist;
        }

        #endregion

        #region GetBySSNumber(string sSNumber

        public async Task<Employee> GetBySSNumber(string sSNumber)
        {
            var employee = applicationDbContext.Employees
                .Where(e => e.SSNumber == sSNumber)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            return await employee;
        }

        #endregion

        #region ActivateEmployeeAsync(Employee employee)

        public async Task ActivateEmployeeAsync(Employee employee)
        {
            employee.IsActive = true;
            var employeeUpdated = applicationDbContext.Employees.Update(employee);
            await applicationDbContext.SaveChangesAsync();
        }

        #endregion

    }
}

