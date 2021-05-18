using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrmApp.Core.Models;
using hrmApp.Core.Repositories;

namespace hrmApp.Data.Repositories
{
    public class DocTypeRepository : Repository<DocType>, IDocTypeRepository
    {
        private ApplicationDbContext applicationDbContext { get => _context as ApplicationDbContext; }

        public DocTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DocType> GetWithDocumentsByIdAsync(int docTypeId)
        {
            var docType = applicationDbContext.DocTypes
                            .Include(x => x.Documents)
                            .SingleOrDefaultAsync(x => x.Id == docTypeId);
            return await docType;
        }
    }
}
