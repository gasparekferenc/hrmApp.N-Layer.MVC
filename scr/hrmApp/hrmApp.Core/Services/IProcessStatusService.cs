using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IProcessStatusService : IService<ProcessStatus>
    {
        Task<ProcessStatus> GetWithEployeesByIdAsync(int processStatusId);
    }
}
