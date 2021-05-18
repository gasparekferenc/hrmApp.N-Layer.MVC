using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hrmApp.Core.PagedList
{
    public static class PagedListExtensions
    {
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageIndex < indexFrom)
            {
                throw new ArgumentException($"pageIndex: {pageIndex} < indexFrom: {indexFrom}. Must pageIndex >= indexFrom!");
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            if ((pageIndex - indexFrom) > pagedList.TotalPages)
            {
                throw new ArgumentException($"pageIndex: {pageIndex}. It must be between indexFrom: {indexFrom} and TotalPages: {pagedList.TotalPages}!");
            }

            return pagedList;
        }
    }
}
