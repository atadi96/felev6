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

        Task<Boolean> CreateArticleAsync(ArticleUploadDTO article);

        Task<Boolean> UpdateArticleAsync(ArticleUploadDTO article);

        Task<Boolean> DeleteArticleAsync(int articleID);

        Task<AuthorDTO> LoginAsync(string userName, string userPassword);

        Task<Boolean> LogoutAsync();
    }
}
