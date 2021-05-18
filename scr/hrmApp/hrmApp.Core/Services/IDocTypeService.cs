using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Services
{
    public interface IDocTypeService : IService<DocType>
    {
        Task<DocType> GetWithDocumentsByIdAsync(int docTypeId);
    }
}