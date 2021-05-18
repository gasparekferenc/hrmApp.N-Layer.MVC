using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IProcessStatusRepository : IRepository<ProcessStatus>
    {
        Task<ProcessStatus> GetWithEployeesByIdAsync(int processStatusId);
    }
}
