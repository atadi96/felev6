using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hirportal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Hirportal.Persistence;

namespace Hirportal.Controllers
{
    public class HomeController : Controller
    {
        NewsContext _context;

        public HomeController(NewsContext context)
        {
            _context = context;
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
            return View("Index", new HomeViewModel(articles, leading));
        }

        public IActionResult Article(int id)
        {
            var article = _context.Articles
                            .Include(a => a.Images)
                            .Include(a => a.Author)
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
            return View("Article", article == null ? null : new ArticleViewModel(article));
        }

        public IActionResult Gallery(int id, PagingViewModel<ImageViewModel> paging)
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
                                .Where(img => img.Article == article)
                                .Select(img => new ImageViewModel(img));
                paging.UpdatePageContents(1, images);
                ViewBag.ArticleTitle = article.Title;
            }
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
