using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class ProjectService : Service<Project>, IProjectService
    {
        public ProjectService(IUnitOfWork unitOfWork, IRepository<Project> repository)
                            : base(unitOfWork, repository)
        {
        }

        public async Task<Project> GetWithPOByIdAsync(int projectId)
        {
            return await _unitOfWork.Projects.GetWithPOByIdAsync(projectId);
        }

        public async Task<IEnumerable<Project>> GetAllWithIncludesAsync()
        {
            return await _unitOfWork.Projects.GetAllWithIncludesAsync();
        }
        public async Task<Project> GetWithIncludesByIdAsync(int projectId)
        {
            return await _unitOfWork.Projects.GetWithIncludesByIdAsync(projectId);
        }
    }
}
