using System.Collections.Generic;
using System.Threading.Tasks;
using hrmApp.Core.Models;

namespace hrmApp.Core.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Document> Get___ByIdAsync(int documentId);
        Task<IEnumerable<Document>> GetAllByEmployeeIdAsync(int employeeId);
    }
}
