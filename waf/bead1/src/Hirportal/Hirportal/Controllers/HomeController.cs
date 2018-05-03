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

        public IActionResult Gallery(int id)
        {
            var article = _context.Articles
                            .Include(a => a.Images)
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
            ViewBag.ImagePath = ImagePath;
            ViewBag.ArticleId = id;
            return View("Gallery", article);
        }

        public IActionResult Archive(ArchiveViewModel archive)
        {
            int articlesPerPage = 20;
            DateTime? dateTime = archive.DateTime;
            string search = archive.Search;
            int page = archive.Page;
            IQueryable<Article> articles =
                _context.Articles
                        .OrderByDescending(art => art.Modified)
                        .Where(art =>
                            String.IsNullOrWhiteSpace(search) ||
                            art.Title.Contains(search) ||
                            art.Content.Contains(search)
                        ).Where(art =>
                            dateTime == null || art.Modified.Date == dateTime.Value.Date
                        );
            long articleNum = articles.LongCount();
            int maxPage = (int)((articleNum - 1) / 20) + 1;
            int actualPage = Math.Min(maxPage, Math.Max(1, page));

            var displayArticles = articles
                                    .Skip((actualPage - 1) * articlesPerPage)
                                    .Take(articlesPerPage)
                                    .ToArray();
            archive.Articles = displayArticles;
            archive.Page = actualPage;
            archive.LastPage = actualPage == maxPage;
            return View("Archive", archive);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
