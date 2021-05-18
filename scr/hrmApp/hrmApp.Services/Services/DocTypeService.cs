using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using hrmApp.Core.UnitOfWorks;
using hrmApp.Core.Services;

namespace hrmApp.Services.Services
{
    public class DocTypeService : Service<DocType>, IDocTypeService
    {
        public DocTypeService(IUnitOfWork unitOfWork, IRepository<DocType> repository)
                            : base(unitOfWork, repository)
        {
        }

        public async Task<DocType> GetWithDocumentsByIdAsync(int docTypeId)
        {
            return await _unitOfWork.DocTypes.GetWithDocumentsByIdAsync(docTypeId);
        }
    }
}
