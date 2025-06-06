using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class OuvinteController : Controller
    {
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
