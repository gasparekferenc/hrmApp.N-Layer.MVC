using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetAllWithIncludesAsync();
        Task<Project> GetWithIncludesByIdAsync(int projectId);
        Task<Project> GetWithPOByIdAsync(int projectId);
    }
}
