using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;

namespace hrmApp.Data.Repositories
{
    public class ProcessStatusRepository : Repository<ProcessStatus>, IProcessStatusRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public ProcessStatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProcessStatus> GetWithEployeesByIdAsync(int processStatusId)
        {
            var processStatus = applicationDbContext.ProcessStatuses
                            .Include(x => x.Employees)
                            .SingleOrDefaultAsync(x => x.Id == processStatusId);
            return await processStatus;
        }
    }
}
