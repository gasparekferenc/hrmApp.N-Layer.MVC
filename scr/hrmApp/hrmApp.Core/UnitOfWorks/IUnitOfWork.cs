using hrmApp.Core.Repositories;
using System.Threading.Tasks;

// Cause to have all Repositories

namespace hrmApp.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUsers { get; }
        IAssignmentRepository Assignments { get; }
        IDocTypeRepository DocTypes { get; }
        IDocumentRepository Documents { get; }
        IEmployeeRepository Employees { get; }
        IHistoryRepository Histories { get; }
        IJobRepository Jobs { get; }
        IOrganizationRepository Organizations { get; }
        IPOEmployeeRepository POEmployees { get; }
        IProcessStatusRepository ProcessStatuses { get; }
        IProjectRepository Projects { get; }
        IProjectOrganizationRepository ProjectOrganizations { get; }


        Task CommitAsync();
        void Commit();
    }
}
