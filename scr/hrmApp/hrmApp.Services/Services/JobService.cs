using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;

namespace hrmApp.Services.Services
{
    public class JobService : Service<Job>, IJobService
    {
        public JobService(IUnitOfWork unitOfWork, IRepository<Job> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<Job> GetWithEployeesByIdAsync(int jobId)
        {
            return await _unitOfWork.Jobs.GetWithEployeesByIdAsync(jobId);
        }
    }
}
