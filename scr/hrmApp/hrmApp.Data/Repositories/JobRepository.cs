using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;

namespace hrmApp.Data.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public JobRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Job> GetWithEployeesByIdAsync(int jobId)
        {
            var job = applicationDbContext.Jobs
                            .Include(x => x.Employees)
                            .SingleOrDefaultAsync(x => x.Id == jobId);
            return await job;
        }
    }
}
