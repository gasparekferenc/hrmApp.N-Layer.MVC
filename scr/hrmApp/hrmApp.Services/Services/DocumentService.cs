using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;
using System.Collections.Generic;

namespace hrmApp.Services.Services
{
    public class DocumentService : Service<Document>, IDocumentService
    {
        public DocumentService(IUnitOfWork unitOfWork, IRepository<Document> repository)
                            : base(unitOfWork, repository)
        {
        }

        public async Task<IEnumerable<Document>> GetAllByEmployeeIdAsync(int employeeId)
        {
            return await _unitOfWork.Documents.GetAllByEmployeeIdAsync(employeeId);
        }

        public async Task<Document> Get___ByIdAsync(int documentId)
        {
            return await _unitOfWork.Documents.Get___ByIdAsync(documentId);
        }
    }
}
