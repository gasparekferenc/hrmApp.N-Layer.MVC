using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IHistoryService : IService<History>
    {
        Task<History> Get___ByIdAsync(int historyId);
        Task<IEnumerable<History>> GetAllByEmployeeIdAsync(int employeeId);
        Task<History> AddEntry(string entry, string entryType, int employeeId, string userId);
        Task<History> AddEntry(string entry, string entryType, int employeeId, string userId, int? documentId);
        Task<History> AddEntry(string entry, string entryType, int employeeId, string userId, int? documentId, bool? isRemainder, DateTime? deadLineDate);
    }
}
