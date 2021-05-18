using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IDocTypeRepository : IRepository<DocType>
    {
        Task<DocType> GetWithDocumentsByIdAsync(int docTypeId);
    }
}
