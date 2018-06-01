using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hirportal.Persistence.DTO;

namespace Hirportal.WPF.Persistence
{
    class MockupPersistence : INewsPersistence
    {
        public bool IsLoggedOn { get; private set; }

        public MockupPersistence()
        {
            IsLoggedOn = false;
        }

        public async Task<bool> CreateArticleAsync(ArticleDTO article)
        {
            article.Id = 10;
            return true;
        }

        public async Task<bool> DeleteArticleAsync(int articleID)
        {
            return true;
        }

        public Task<ArticleDTO> GetArticleAsync(int articleID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ArticlePreviewDTO>> GetUserArticlesAsync()
        {
            return new ArticlePreviewDTO[]
            {
                new ArticlePreviewDTO()
                {
                    Id = 0,
                    Author = "Gloria Borger",
                    PublishedTime = DateTime.Now,
                    Title = "Am I PewDiePie?"
                },
                new ArticlePreviewDTO()
                {
                    Id = 1,
                    Author = "Gloria Borger",
                    PublishedTime = DateTime.Now,
                    Title = "Weekly highlights"
                }
            };
        }

        public async Task<AuthorDTO> LoginAsync(string userName, string userPassword)
        {
            if (userName == "admin" && userPassword == "admin")
            {
                IsLoggedOn = true;
                return new AuthorDTO() { Name = "Gloria Borger", Username = "pdp" };
            }
            else
            {
                IsLoggedOn = false;
                return null;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            IsLoggedOn = false;
            return true;
        }

        public async Task<bool> UpdateArticleAsync(ArticleDTO article)
        {
            return true;
        }
    }
}
