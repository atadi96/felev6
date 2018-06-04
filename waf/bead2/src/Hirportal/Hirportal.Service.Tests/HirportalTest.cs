using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Hirportal.Service.Controllers;
using Hirportal.Persistence;
using Hirportal.Persistence.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Net.Http;

namespace Hirportal.Service.Tests
{
    public class HirportalTest : IClassFixture<ClientServerFixture>
    {
        ClientServerFixture fixture;

        public HirportalTest(ClientServerFixture fxt)
        {
            fixture = fxt;
        }

        [Fact]
        public async Task CantSeeArticlesWithoutLoggingIn()
        {
            var response = await fixture.Client.GetAsync("api/articles/");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task TestCanLogin()
        {
            var response = await fixture.Client.GetAsync("api/account/login/test/test");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task LoggedInCanSeeArticles()
        {
            var response = await fixture.Client.GetAsync("api/articles/");

            response.EnsureSuccessStatusCode();
            Assert.Single(await response.Content.ReadAsAsync<IEnumerable<ArticlePreviewDTO>>());
        }

        [Fact]
        public async Task CanPostValidArticle()
        {
            var response = await fixture.Client.PostAsJsonAsync<ArticleUploadDTO>("api/articles/", new ArticleUploadDTO()
            {
                Content = "Valid content",
                DeleteImages = false,
                Description = "Valid description",
                Leading = false,
                NewImages = new byte[0][],
                Title = "Valid title"
            });

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CantPostInvalidArticle()
        {
            var response = await fixture.Client.PostAsJsonAsync<ArticleUploadDTO>("api/articles/", new ArticleUploadDTO()
            {
                Content = "Valid content",
                DeleteImages = false,
                Description = "Valid description",
                Leading = true,
                NewImages = new byte[0][],
                Title = "Valid title"
            });

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
