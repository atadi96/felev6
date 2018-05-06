using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hirportal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hirportal.Controllers
{
    public class HomeController : Controller
    {
        NewsContext _context;
        IOptions<ImagePathConfig> _imagePath;

        private string ImagePath => "/" + _imagePath.Value.ImagePath;

        public HomeController(NewsContext context, IOptions<ImagePathConfig> pathConfig)
        {
            _context = context;
            _imagePath = pathConfig;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles
                            .OrderByDescending(art => art.Modified)
                            .Where(art => !art.Leading)
                            .Take(10)
                            .ToArray();
            var leading = _context.Articles
                            .Include(art => art.Images)
                            .OrderByDescending(art => art.Modified)
                            .Where(art => art.Leading)
                            .FirstOrDefault(_ => true);
            return View("Index", new HomeViewModel(ImagePath, articles,leading));
        }

        public IActionResult Article(int id)
        {
            var article = _context.Articles
                            .Include(a => a.Images)
                            .Include(a => a.Author)
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
            ViewBag.ImagePath = ImagePath;
            return View("Article", article);
        }

        public IActionResult Gallery(int id, PagingViewModel<ArticleImage> paging)
        {
            var article = _context.Articles
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
            if (article == null)
            {
                paging = null;
            }
            else
            {
                var images = _context.Images
                                .Where(img => img.Article == article);
                paging.UpdatePageContents(1, images);
                ViewBag.ArticleTitle = article.Title;
            }
            ViewBag.ImagePath = ImagePath;
            ViewBag.ArticleId = id;
            return View("Gallery", paging);
        }

        public IActionResult Archive(ArchiveViewModel archive)
        {
            int articlesPerPage = 20;
            DateTime? dateTime = archive.DateTime;
            IQueryable<Article> articles =
                _context.Articles
                        .OrderByDescending(art => art.Modified)
                        .Where(art => ( String.IsNullOrWhiteSpace(archive.TitleSearch) ||
                                        art.Title.Contains(archive.TitleSearch) )
                                   && ( String.IsNullOrWhiteSpace(archive.ContentSearch) ||
                                        art.Content.Contains(archive.ContentSearch) )
                        ).Where(art =>
                            dateTime == null || art.Modified.Date == dateTime.Value.Date
                        );
            archive.UpdatePageContents(articlesPerPage, articles);
            return View("Archive", archive);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
