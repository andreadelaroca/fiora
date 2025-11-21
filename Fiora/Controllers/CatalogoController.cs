using Microsoft.AspNetCore.Mvc;

namespace Fiora.Controllers
{
    public class CatalogoController : Controller
    {
        // Acción principal del catálogo
        public IActionResult Index()
        {
            return View();
        }
    }
}
