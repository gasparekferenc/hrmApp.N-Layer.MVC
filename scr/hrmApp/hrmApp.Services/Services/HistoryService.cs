using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using hrmApp.Core.Constants;
using System;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class HistoryService : Service<History>, IHistoryService
    {
        public HistoryService(IUnitOfWork unitOfWork, IRepository<History> repository)
                            : base(unitOfWork, repository)
        {
        }
        public async Task<History> Get___ByIdAsync(int historyId)
        {
            return await _unitOfWork.Histories.Get___ByIdAsync(historyId);
        }

        public async Task<IEnumerable<History>> GetAllByEmployeeIdAsync(int employeeId)
        {
            return await _unitOfWork.Histories.GetAllByEmployeeIdAsync(employeeId);
        }

        public async Task<History> AddEntry(string entry, string entryType, int employeeId, string userId)
        {
            return await AddEntry(entry, entryType, employeeId, userId, null);
        }
        public async Task<History> AddEntry(string entry, string entryType, int employeeId, string userId, int? documentId)
        {
            // var historyEntry = new History
            // {
            //     Entry = entry,
            //     EntryType = EntryTypes.Names.IndexOf(entryType),
            //     EntryDate = DateTime.Now,
            //     AppUserEntry = false,
            //     DocumentId = documentId,
            //     IsReminder = false,
            //     DeadlineDate = null,
            //     EmployeeId = employeeId,
            //     ApplicationUserId = userId
            // };

            // await base.AddAsync(historyEntry);
            // await _unitOfWork.Histories.AddAsync(historyEntry);
            // await _unitOfWork.CommitAsync();

            // return historyEntry;

            return await AddEntry(entry, entryType, employeeId, userId, null, null, null);
        }
        public async Task<History> AddEntry(string entry, string entryType, int employeeId, string userId, int? documentId, bool? isRemainder, DateTime? deadLineDate)
        {
            if (isRemainder == null || isRemainder == false)
            {
                isRemainder = false;
                deadLineDate = null;
            };

            var historyEntry = new History
            {
                Entry = entry,
                EntryType = EntryTypes.Names.IndexOf(entryType),
                EntryDate = DateTime.Now,
                AppUserEntry = true,
                DocumentId = documentId,
                IsReminder = (bool)isRemainder,
                DeadlineDate = deadLineDate,
                EmployeeId = employeeId,
                ApplicationUserId = userId
            };

            await base.AddAsync(historyEntry);

            return historyEntry;
        }
    }
}
