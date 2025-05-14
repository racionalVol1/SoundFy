using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class RegistroController(UsuarioRepository usuarioRepository) : Controller
    {
        private readonly UsuarioRepository _usuarioRepository = usuarioRepository;

        public IActionResult Index()
        {
            return View();
        }     

        [HttpPost]
        public IActionResult Registrar(string email, string senha)
        {
            if (_usuarioRepository.RegistrarUsuario(email, senha))
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