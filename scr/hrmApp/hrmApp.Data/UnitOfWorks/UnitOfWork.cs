using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using hrmApp.Data.Repositories;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;

namespace hrmApp.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        private ApplicationUserRepository _applicationUserRepository;
        private AssignmentRepository _assignmentRepository;
        private DocTypeRepository _docTypeRepository;
        private DocumentRepository _documentRepository;
        private EmployeeRepository _employeeRepository;
        private HistoryRepository _historyRepository;
        private JobRepository _jobRepository;
        private OrganizationRepository _organizationRepository;
        private POEmployeeRepository _poEmployeeRepository;
        private ProcessStatusRepository _processStatusRepository;
        private ProjectOrganizationRepository _projectOrganizationRepository;
        private ProjectRepository _projectRepository;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        // To trying 'arrow function' syntax
        public IApplicationUserRepository ApplicationUsers => _applicationUserRepository ??= new ApplicationUserRepository(_context);
        public IAssignmentRepository Assignments => _assignmentRepository ??= new AssignmentRepository(_context);
        public IDocTypeRepository DocTypes => _docTypeRepository ??= new DocTypeRepository(_context);
        public IDocumentRepository Documents => _documentRepository ??= new DocumentRepository(_context);
        public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_context);
        public IHistoryRepository Histories => _historyRepository ??= new HistoryRepository(_context);
        public IJobRepository Jobs => _jobRepository ??= new JobRepository(_context);
        public IOrganizationRepository Organizations => _organizationRepository ??= new OrganizationRepository(_context);
        public IPOEmployeeRepository POEmployees => _poEmployeeRepository ??= new POEmployeeRepository(_context);
        public IProcessStatusRepository ProcessStatuses => _processStatusRepository ??= new ProcessStatusRepository(_context);
        public IProjectOrganizationRepository ProjectOrganizations => _projectOrganizationRepository ??= new ProjectOrganizationRepository(_context);
        public IProjectRepository Projects => _projectRepository ??= new ProjectRepository(_context);



        public void Commit()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                _context.Dispose();
                transaction.Rollback();
                Log.Information($"Nem sikerült végrehajtani a tranzakció! '{e}'");
            }
        }

        public async Task CommitAsync()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateConcurrencyException e)
            {
                transaction.Rollback();
                Log.Information($"Nem sikerült végrehajtani a tranzakció (Concurrency)! '{e}'");
                throw e; // I don't handle that error here, so rethrow
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                Log.Information($"Nem sikerült végrehajtani a tranzakció (Constraint violation)! '{e}'");
                throw e; // I don't handle that error here, so rethrow
            }

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

