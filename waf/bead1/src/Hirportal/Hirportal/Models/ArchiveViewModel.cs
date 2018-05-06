using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class ArchiveViewModel : PagingViewModel<Article>
    {
        public ArchiveViewModel() : base()
        {
            TitleSearch = "";
            ContentSearch = "";
            DateTime = null;
        }

        public Article[] Articles => Items;
        public string TitleSearch { get; set; }
        public string ContentSearch { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
