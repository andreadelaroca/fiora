using System.Diagnostics;
using Fiora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fiora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            // Serve the static Home page from Views/Home/Index.html
            var path = Path.Combine(_env.ContentRootPath, "Views", "Home", "Index.html");
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, "text/html; charset=utf-8");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Serve the static Catalog page from Views/Home/catalogo.html
        [HttpGet("catalogo.html")]
        public IActionResult CatalogoHtml()
        {
            var path = Path.Combine(_env.ContentRootPath, "Views", "Home", "catalogo.html");
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, "text/html; charset=utf-8");
            }
            return NotFound();
        }

        // Serve the static Contact page from Views/Home/contacto.html
        [HttpGet("contacto.html")]
        public IActionResult ContactoHtml()
        {
            var path = Path.Combine(_env.ContentRootPath, "Views", "Home", "contacto.html");
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, "text/html; charset=utf-8");
            }
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 404 Not Found page
        [HttpGet("404")]
        public IActionResult NotFoundPage()
        {
            var path = Path.Combine(_env.ContentRootPath, "Views", "Home", "404.html");
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, "text/html; charset=utf-8");
            }
            return NotFound("404 page not found.");
        }
    }
}
