using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using hrmApp.Core.PagedList;
using hrmApp.Core.Constants;
using Microsoft.EntityFrameworkCore.Query;

namespace hrmApp.Core.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAysnc(int Id);
        Task<TEntity> GetByIdAysnc(string Id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);

        // Nem használjuk...
        Task<IPagedList<TEntity>> GetPagedListAsync(
                Expression<Func<TEntity, bool>> predicate = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                int pageIndex = 0,
                int pageSize = Paging.PageSize,
                bool disableTracking = true,
                CancellationToken cancellationToken = default(CancellationToken),
                bool ignoreQueryFilters = false);
    }
}
