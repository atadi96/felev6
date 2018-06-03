using System;
using System.Collections.Generic;
using System.Text;

namespace Hirportal.Persistence.DTO
{
    public class ArticleUploadDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public bool DeleteImages { get; set; }

        public byte[][] NewImages { get; set; }

        public bool Leading { get; set; }
    }
}
