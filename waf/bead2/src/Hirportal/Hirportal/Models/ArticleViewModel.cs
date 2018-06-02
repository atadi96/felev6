using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hirportal.Persistence;

namespace Hirportal.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Author { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Content { get; set; }
        
        public DateTime Modified { get; set; }

        public ImageViewModel Image { get; set; }

        public ArticleViewModel() { }

        public ArticleViewModel(Article article)
        {
            Id = article.Id;
            Author = article.Author.Name;
            Title = article.Title;
            Description = article.Description;
            Content = article.Content;
            Modified = article.Modified;
            var firstImage = article.Images.FirstOrDefault();
            if (firstImage != null)
            {
                Image = new ImageViewModel(firstImage);
            }
            else
            {
                Image = null;
            }
        }
    }
}
