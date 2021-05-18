using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;

namespace hrmApp.Services.Services
{
    public class ProcessStatusService : Service<ProcessStatus>, IProcessStatusService
    {
        public ProcessStatusService(IUnitOfWork unitOfWork, IRepository<ProcessStatus> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<ProcessStatus> GetWithEployeesByIdAsync(int processStatusId)
        {
            return await _unitOfWork.ProcessStatuses.GetWithEployeesByIdAsync(processStatusId);
        }
    }
}
