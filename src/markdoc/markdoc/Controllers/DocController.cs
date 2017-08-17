using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using markdoc.Data;
using Realms;

namespace markdoc.Controllers
{
    public class DocController : Controller
    {
        private readonly IHostingEnvironment _HostingEnvironment;
        private Realm _realm;

        public DocController(IHostingEnvironment hostingEnvironment, Realm realm)
        {
            _HostingEnvironment = hostingEnvironment;
            _realm = realm;
        }

        public IActionResult Index()
        {

            var docs = _realm.All<Document>().ToList();

            //if (!docs.Any()) 
                //realm.Write(() => {realm.Add(new Document() { Title = "Test Doc" }); });
            return View(docs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Document());
        }

        [HttpPost]
        public IActionResult Create(Document doc){

            _realm.Write(() => _realm.Add(doc));

            return RedirectToAction("ViewDoc", new { doc.Id });
        }

        [HttpGet]
        public IActionResult Edit(string id){
            return View("Create", _realm.Find<Document>(id));
        }

        [HttpPost]
        public IActionResult Edit(string id, Document doc){
            _realm.Write(() => _realm.Add(doc, update: true));

            return RedirectToAction("ViewDoc", new { doc.Id });
        }

        public IActionResult ViewDoc(string id){
            return View(_realm.Find<Document>(id));
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
