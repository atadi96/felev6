using System;
using System.Collections.Generic;
using System.Text;

namespace Hirportal.Persistence.DTO
{
    public class ArticlePreviewDTO
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public DateTime PublishedTime { get; set; }
    }
}
