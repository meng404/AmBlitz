using System.Collections.Generic;
using System.Linq;

namespace AmBlitzCore.ToolKit
{
    public class Pagination<T>
    {
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Datas { get; set; }

        public static Pagination<T> ToPage(IQueryable<T> source, int pageIndex=1, int pageSize=500)
        {
            var page = new Pagination<T> { TotalCount = source.Count() };

            page.TotalPages = page.TotalCount / pageSize;
            if (page.TotalCount % pageSize > 0)
            {
                page.TotalPages++;
            }
            page.PageSize = pageSize;
            page.PageIndex = pageIndex;
            var res = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            page.Datas = res.Any() ? res.ToList() : new List<T>();
            page.HasPreviousPage = pageIndex - 1 > 0;
            page.HasNextPage = pageIndex < page.TotalPages;
            return page;
        }
    }
}
