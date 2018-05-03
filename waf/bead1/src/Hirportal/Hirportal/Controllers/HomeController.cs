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
            return View("Index", new HomeViewModel(_imagePath.Value.ImagePath, articles,leading));
        }

        public IActionResult Article(int id)
        {
            var article = _context.Articles
                            .Include(a => a.Images)
                            .Include(a => a.Author)
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
            ViewBag.ImagePath = _imagePath.Value.ImagePath;
            return View("Article", article);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
