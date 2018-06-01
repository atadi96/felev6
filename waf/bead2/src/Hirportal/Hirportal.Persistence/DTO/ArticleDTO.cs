using System;
using System.Collections.Generic;
using System.Text;

namespace Hirportal.Persistence.DTO
{
    public class ArticleDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public byte[] LeadingImage { get; set; }

        public List<byte[]> Images { get; set; }
    }
}
