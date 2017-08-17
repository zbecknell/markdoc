using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace markdoc.Controllers
{
    public class DocController : Controller
    {
        private readonly IHostingEnvironment _HostingEnvironment;

        public DocController(IHostingEnvironment hostingEnvironment)
        {
            _HostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            var webRootPath = _HostingEnvironment.WebRootPath;

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(webRootPath, @"uploads", fileName);

            if(file.Length > 0)
            {
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var path = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}/uploads/{fileName}";

            return Ok(new { size = file.Length, path });
        }
    }
}
