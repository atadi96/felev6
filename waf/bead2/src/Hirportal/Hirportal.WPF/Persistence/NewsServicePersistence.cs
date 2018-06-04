using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Hirportal.Persistence.DTO;

namespace Hirportal.WPF.Persistence
{
    public class NewsServicePersistence : INewsPersistence
    {
        private const string API_ARTICLES = "api/articles/";

        private const string API_ACCOUNT = "api/account/";

        private HttpClient _client;

        public bool IsLoggedOn { get; private set; }

        public NewsServicePersistence(String baseAddress)
        {
            _client = new HttpClient(); // a szolgáltatás kliense
            _client.BaseAddress = new Uri(baseAddress); // megadjuk neki a címet
            IsLoggedOn = false;
        }

        public async Task<IEnumerable<ArticlePreviewDTO>> GetUserArticlesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ARTICLES);
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ArticlePreviewDTO> previews = await response.Content.ReadAsAsync<IEnumerable<ArticlePreviewDTO>>();
                    return previews;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }

        }

        public async Task<ArticleDTO> GetArticleAsync(int articleID)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ARTICLES + articleID);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsAsync<ArticleDTO>());
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (PersistenceUnavailableException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> CreateArticleAsync(ArticleUploadDTO articleDTO)
        {
            try
            {
                using (HttpResponseMessage response = await _client.PostAsJsonAsync(API_ARTICLES, articleDTO))
                { // az értékeket azonnal JSON formátumra alakítjuk
                    articleDTO.Id = (await response.Content.ReadAsAsync<ArticleDTO>()).Id; // a válaszüzenetben megkapjuk a végleges azonosítót
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> UpdateArticleAsync(ArticleUploadDTO articleDTO)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync(API_ARTICLES, articleDTO);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> DeleteArticleAsync(int articleID)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(API_ARTICLES + articleID);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<AuthorDTO> LoginAsync(string userName, string userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ACCOUNT + "login/" + userName + "/" + userPassword);
                if (response.IsSuccessStatusCode)
                {
                    AuthorDTO result = await response.Content.ReadAsAsync<AuthorDTO>();
                    IsLoggedOn = true;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ACCOUNT + "logout");
                IsLoggedOn = !response.IsSuccessStatusCode;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
