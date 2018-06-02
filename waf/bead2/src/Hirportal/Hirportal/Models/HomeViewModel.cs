using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hirportal.Persistence;

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
            public LeadingArticleViewModel(Article article)
            {
                Article = new ArticleViewModel(article);
                var leadingImage = article.Images.First();
                Image = new ImageViewModel(leadingImage);
            }

            public ImageViewModel Image { get; private set; }
            public ArticleViewModel Article { get; private set; }
        }

        public ArticleViewModel[] Articles { get; private set; }
        public LeadingArticleViewModel LeadingArticle { get; private set; }

        public HomeViewModel(Article[] articles, Article leading)
        {
            Articles = articles
                        ?.Select(article => new ArticleViewModel(article))
                        ?.ToArray()
                        ?? new ArticleViewModel[0];
            LeadingArticle = leading == null
                                ? null
                                : new LeadingArticleViewModel(leading);
        }
    }
}
