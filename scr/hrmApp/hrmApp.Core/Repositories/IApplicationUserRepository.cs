using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> GetAllWithIncludesAsync();
        Task<ApplicationUser> GetWithIncludesByIdAsync(string applicationUserId);
        Task<ApplicationUser> GetWithAssignmentsByIdAsync(string applicationUserId);
        Task<IEnumerable<Project>> GetProjectsOfUserAsync(string applicationUserId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}
