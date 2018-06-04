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
using System.Drawing;
using System.IO;

namespace Hirportal.Service.Controllers
{
    [Route("api/articles/")]
    public class ArticlesController : Controller
    {
        private NewsContext newsContext;
        private UserManager<Author> userManager;

        public ArticlesController(NewsContext newsContext, UserManager<Author> userManager)
        {
            this.newsContext = newsContext;
            this.userManager = userManager;
        }

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
            try
            {
                Author author = await GetCurrentAuthor();
                Article article =
                    newsContext.Articles
                        .Include(art => art.Author)
                        .Include(art => art.Images)
                        .Where(art => art.Author == author && art.Id == id)
                        .FirstOrDefault();
                if (article == null)
                {
                    return NotFound();
                }
                else
                {
                    ArticleDTO result = new ArticleDTO()
                    {
                        Content = article.Content,
                        Description = article.Description,
                        Id = article.Id,
                        Images = article.Images
                                    .OrderBy(image => image.Id)
                                    .Select(img => new ImageDTO(img.ImageData))
                                    .ToArray(),
                        Title = article.Title,
                        Leading = article.Leading
                    };
                    return Ok(result);
                }
            }catch
            {
                return StatusCode(500);
            }
        }

        private static ArticleImage[] TryCreateImages(byte[][] data, Article article)
        {
            ArticleImage[] images = new ArticleImage[data.Length];
            bool success = true;
            for (int i = 0; success && i < data.Length; i++)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(data[i]))
                    {
                        Image image = Image.FromStream(ms);
                        var jpg = System.Drawing.Imaging.ImageFormat.Jpeg;
                        using (var outStream = new MemoryStream())
                        {
                            image.Save(outStream, jpg);
                            images[i] = new ArticleImage()
                            {
                                ImageData = outStream.ToArray(),
                                Article = article
                            };
                        }
                    }
                }
                catch
                {
                    success = false;
                }
            }
            if (success)
            {
                return images;
            }
            else
            {
                return null;
            }
        }

        // POST api/articles
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ArticleUploadDTO articleDTO)
        {
            try
            {
                if (articleDTO.Leading && articleDTO.NewImages.Length < 1)
                {
                    ModelState.AddModelError("", "At least one image is required for a leading article");
                }
                if (ModelState.IsValid)
                {
                    Article article = new Article()
                    {
                        Content = articleDTO.Content,
                        Leading = articleDTO.Leading,
                        Title = articleDTO.Title,
                        Description = articleDTO.Description
                    };
                    ArticleImage[] images = TryCreateImages(articleDTO.NewImages, article);
                    if (images == null)
                    {
                        ModelState.AddModelError("", "An image was invalid format");
                    }
                    else
                    {
                        article.Author = await GetCurrentAuthor();
                        var result = await newsContext.Articles.AddAsync(article);
                        await newsContext.Images.AddRangeAsync(images);
                        await newsContext.SaveChangesAsync();
                        return Ok(new ArticleDTO()
                        {
                            Content = result.Entity.Content,
                            Description = result.Entity.Description,
                            Id = result.Entity.Id,
                            Images = null,
                            Title = result.Entity.Title,
                            Leading = result.Entity.Leading
                        });
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // PUT api/articles/5
        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody]ArticleUploadDTO articleDTO)
        {
            try
            {
                Author author = await GetCurrentAuthor();
                Article original = newsContext.Articles
                                        .Include(article => article.Author)
                                        .Include(article => article.Images)
                                        .Where(article => article.Author == author && article.Id == articleDTO.Id)
                                        .FirstOrDefault();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (original == null)
                {
                    ModelState.AddModelError("", "No article by given author by given id.");
                    return BadRequest();
                }
                if (articleDTO.Leading)
                {
                    if (articleDTO.DeleteImages)
                    {
                        if (articleDTO.NewImages.Length < 1)
                        {
                            ModelState.AddModelError("", "At least one image is required for a leading article");
                            return BadRequest();
                        }
                    }
                    else
                    {
                        if (articleDTO.NewImages.Length + original.Images.ToArray().Length < 1)
                        {
                            ModelState.AddModelError("", "At least one image is required for a leading article");
                            return BadRequest();
                        }
                    }
                }
                ArticleImage[] newImages = TryCreateImages(articleDTO.NewImages, original);
                if (newImages == null)
                {
                    ModelState.AddModelError("", "An image was invalid format");
                    return BadRequest();
                }

                if (articleDTO.DeleteImages)
                {
                    foreach (var image in original.Images)
                    {
                        newsContext.Images.Remove(image);
                    }
                    original.Images.Clear();
                }
                newsContext.Images.AddRange(newImages);
                original.Title = articleDTO.Title;
                original.Leading = articleDTO.Leading;
                original.Description = articleDTO.Description;
                original.Content = articleDTO.Content;
                newsContext.Articles.Update(original);
                await newsContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        // DELETE api/articles/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Author author = await GetCurrentAuthor();
                Article original = newsContext.Articles
                                        .Include(article => article.Author)
                                        .Include(article => article.Images)
                                        .Where(article => article.Author == author && article.Id == id)
                                        .FirstOrDefault();
                if (original == null)
                {
                    ModelState.AddModelError("", "No article by given author by given id.");
                    return BadRequest();
                }
                else
                {
                    foreach (var image in original.Images)
                    {
                        newsContext.Images.Remove(image);
                    }
                    newsContext.Articles.Remove(original);
                    newsContext.SaveChanges();
                    return Ok();
                }
            }catch
            {
                return StatusCode(500);
            }
        }
    }
}
