using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class PaginaInicialController : Controller
    {
        //Retorno de view da pagina inicial
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            return View(); 
        }
    }
}
   