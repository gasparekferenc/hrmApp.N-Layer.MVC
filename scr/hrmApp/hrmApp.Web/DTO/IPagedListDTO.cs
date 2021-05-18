using System.Collections.Generic;

namespace hrmApp.Web.DTO
{
    public interface IPagedListDTO<T>
    {
        int IndexFrom { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
        int TotalPages { get; set; }
        IList<T> Items { get; set; }
        bool HasPreviousPage { get; set; }
        bool HasNextPage { get; set; }
    }
}
