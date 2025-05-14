using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class RegistroController() : Controller   {
        
        public IActionResult Index()
        {
            return View();
        }     

        [HttpPost]
        public IActionResult Registrar(string email, string senha)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            if (usuarioRepository.RegistrarUsuario(email, senha))
            {
                return RedirectToAction("Index", "login");
            }
            else
            {
                ViewBag.Mensagem = "Erro ao registrar usuário.";
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult Registar()
        {
            return View();
        }
    }
}