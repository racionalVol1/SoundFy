using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class PaginaInicialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
   