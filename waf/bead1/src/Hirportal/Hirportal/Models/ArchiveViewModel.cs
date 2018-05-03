using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class ArchiveViewModel
    {
        public ArchiveViewModel()
        {
            Page = 1;
            Search = null;
            DateTime = null;
        }

        public Article[] Articles { get; set; }
        public int Page { get; set; }
        public bool LastPage { get; set; }
        public bool FirstPage => Page <= 1;
        public string Search { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
