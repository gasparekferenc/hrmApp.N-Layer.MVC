using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class ApplicationUserService : Service<ApplicationUser>, IApplicationUserService
    {
        public ApplicationUserService(IUnitOfWork unitOfWork, IRepository<ApplicationUser> repository)
                                    : base(unitOfWork, repository)
        {
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllWithIncludesAsync()
        {
            return await _unitOfWork.ApplicationUsers.GetAllWithIncludesAsync();
        }
        public async Task<ApplicationUser> GetWithIncludesByIdAsync(string applicationUserId)
        {
            return await _unitOfWork.ApplicationUsers.GetWithIncludesByIdAsync(applicationUserId);
        }
        public async Task<ApplicationUser> GetWithAssignmentsByIdAsync(string applicationUserId)
        {
            return await _unitOfWork.ApplicationUsers.GetWithAssignmentsByIdAsync(applicationUserId);
        }

        public async Task<IEnumerable<Project>> GetProjectsOfUserAsync(string applicationUserId)
        {
            return await _unitOfWork.ApplicationUsers.GetProjectsOfUserAsync(applicationUserId);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.ApplicationUsers.GetUserByEmailAsync(email);
        }
    }
}
