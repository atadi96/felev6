using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class HomeViewModel
    {
        public class ArticleViewModel
        {
            public ArticleViewModel(Article article)
            {
                Id = article.Id;
                Title = article.Title;
                Description = article.Description;
            }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class LeadingArticleViewModel
        {
            public LeadingArticleViewModel(string imgPath, Article article)
            {
                Article = new ArticleViewModel(article);
                ImagePath = System.IO.Path.Combine(imgPath, article.Images.First().SmallPath);
            }

            public string ImagePath { get; private set; }
            public ArticleViewModel Article { get; private set; }
        }

        public ArticleViewModel[] Articles { get; private set; }
        public LeadingArticleViewModel LeadingArticle { get; private set; }

        public HomeViewModel(string imagePath, Article[] articles, Article leading)
        {
            Articles = articles
                        ?.Select(art => new ArticleViewModel(art))
                        ?.ToArray()
                        ?? new ArticleViewModel[0];
            LeadingArticle = leading == null
                                ? null
                                : new LeadingArticleViewModel(imagePath, leading);
        }
    }
}
