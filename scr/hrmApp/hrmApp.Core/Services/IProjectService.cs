using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IProjectService : IService<Project>
    {
        Task<IEnumerable<Project>> GetAllWithIncludesAsync();
        Task<Project> GetWithIncludesByIdAsync(int projectId);
        Task<Project> GetWithPOByIdAsync(int projectId);
    }
}
