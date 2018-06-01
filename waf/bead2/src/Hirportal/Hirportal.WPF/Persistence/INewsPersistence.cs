using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hirportal.Persistence.DTO;

namespace Hirportal.WPF.Persistence
{
    public interface INewsPersistence
    {
        bool IsLoggedOn { get; }

        Task<IEnumerable<ArticlePreviewDTO>> GetUserArticlesAsync();

        Task<ArticleDTO> GetArticleAsync(int articleID);

        Task<Boolean> CreateArticleAsync(ArticleDTO article);

        Task<Boolean> UpdateArticleAsync(ArticleDTO article);

        Task<Boolean> DeleteArticleAsync(int articleID);

        Task<AuthorDTO> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
}
