using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IJobService : IService<Job>
    {
        Task<Job> GetWithEployeesByIdAsync(int jobId);
    }
}
