using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class PagingViewModel<T>
    {
        public PagingViewModel()
        {
            Page = 1;
            MaxPage = 1;
            Items = Array.Empty<T>();
            LastPage = true;
        }

        public T[] Items { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
        public bool LastPage { get; set; }
        public bool FirstPage => Page <= 1;

        public void UpdatePageContents(int itemsPerPage, IQueryable<T> query)
        {
            long itemNum = query.LongCount();
            int maxPage = (int)((itemNum - 1) / itemsPerPage) + 1;
            int actualPage = Math.Min(maxPage, Math.Max(1, Page));
            Items = query.Skip((actualPage - 1) * itemsPerPage)
                         .Take(itemsPerPage)
                         .ToArray();
            Page = actualPage;
            LastPage = actualPage == maxPage;
            MaxPage = maxPage;
        }
    }
}
