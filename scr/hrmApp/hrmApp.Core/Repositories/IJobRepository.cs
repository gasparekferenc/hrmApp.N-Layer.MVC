using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<Job> GetWithEployeesByIdAsync(int jobId);
    }
}
