using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;

namespace hrmApp.Data.Repositories
{
    public class HistoryRepository : Repository<History>, IHistoryRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public HistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<History> Get___ByIdAsync(int historyId)
        {
            var history = applicationDbContext.Histories
                            .Include(x => x.ApplicationUser)
                            .Include(x => x.Employee)
                            .SingleOrDefaultAsync(x => x.Id == historyId);
            return await history;
        }

        public async Task<IEnumerable<History>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var histories = applicationDbContext.Histories
                                .Include(x => x.ApplicationUser)
                                .Include(x => x.Employee)
                                .Where(x => x.EmployeeId == employeeId)
                                .OrderBy(x => x.EntryDate)
                                .AsNoTracking()
                                .ToListAsync();
            return await histories;
            //throw new System.NotImplementedException();
        }
    }
}
