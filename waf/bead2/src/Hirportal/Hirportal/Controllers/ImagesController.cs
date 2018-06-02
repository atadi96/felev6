using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Hirportal.Persistence;

namespace Hirportal.Controllers
{
    public class ImagesController : Controller
    {
        NewsContext context;

        public ImagesController(NewsContext context)
        {
            this.context = context;
        }

        private byte[] GetImageData(int id, ImageSize imageSize = ImageSize.Full)
        {
            return context.Images.Where(img => img.Id == id).FirstOrDefault()?.SizedData(imageSize);
        }

        private IActionResult GetImageResult(int id, ImageSize imageSize = ImageSize.Full)
        {
            var result = GetImageData(id, imageSize);
            return result == null ? (IActionResult)NotFound(id) : File(result, "image/jpg");
        }

        public IActionResult Full(int id)
        {
            return GetImageResult(id);
        }

        public IActionResult Medium(int id)
        {
            return GetImageResult(id, ImageSize.Medium);
        }

        public IActionResult Small(int id)
        {
            return GetImageResult(id, ImageSize.Small);
        }
    }
}
