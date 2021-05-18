using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace hrmApp.Data.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Document>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var documents = applicationDbContext.Documents
                                                //.AllAsync(d => d.EmployeeId == employeeId)
                                                .Where(d => d.EmployeeId == employeeId)
                                                .Include(d => d.DocType)
                                                .AsNoTracking()
                                                .ToListAsync();
            return await documents;
        }
        public async Task<Document> Get___ByIdAsync(int documentId)
        {
            var document = applicationDbContext.Documents
                                                .SingleOrDefaultAsync(x => x.Id == documentId);
            return await document;
        }
    }
}
