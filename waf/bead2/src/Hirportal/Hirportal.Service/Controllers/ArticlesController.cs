using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Hirportal.Persistence;
using Hirportal.Persistence.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hirportal.Service.Controllers
{
    [Route("api/articles/")]
    public class ArticlesController : Controller
    {
        private NewsContext newsContext;
        private UserManager<Author> userManager;

        private async Task<Author> GetCurrentAuthor()
        {
            string userName = User.Identity.Name;
            return await userManager.FindByNameAsync(userName);
        }

        // GET api/articles
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<ArticlePreviewDTO>> Get()
        {
            var author = await GetCurrentAuthor();
            var articles = newsContext.Articles
                            .Include(article => article.Author)
                            .Where(article => article.Author.UserName == author.UserName)
                            .OrderByDescending(article => article.Modified);
            return articles.Select(article => new ArticlePreviewDTO()
            {
                Author = author.Name,
                Id = article.Id,
                PublishedTime = article.Modified,
                Title = article.Title
            });
        }

        // GET api/articles/5
        [Authorize]
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(int id)
        {
            Author author = await GetCurrentAuthor();
            Article article =
                newsContext.Articles
                    .Include(art => art.Author)
                    .Include(art => art.Images)
                    .Where(art => art.Author.UserName == author.UserName && art.Id == id)
                    .FirstOrDefault();
            if (article == null)
            {
                return NotFound();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // POST api/articles
        [Authorize]
        [HttpPost]
        public int Post([FromBody]ArticleDTO value)
        {
            return -1;
        }

        // PUT api/articles/5
        [Authorize]
        [HttpPut()]
        public void Put([FromBody]ArticleDTO value)
        {
        }

        // DELETE api/articles/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
