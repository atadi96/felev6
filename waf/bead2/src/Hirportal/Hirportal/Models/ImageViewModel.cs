using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hirportal.Persistence;

namespace Hirportal.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }

        public string Hash { get; set; }

        public ImageViewModel() { }

        public ImageViewModel(ArticleImage image)
        {
            Id = image.Id;
            Hash = image.ImageHash();
        }

        public object Anonym => new { id = Id, hash = Hash };
    }
}
