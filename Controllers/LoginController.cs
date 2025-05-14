using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class LoginController(UsuarioRepository usuarioRepository) : Controller
    {
        private readonly UsuarioRepository _usuarioRepository = usuarioRepository;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Autenticar(string email, string senha)
        {
            if (_usuarioRepository.ValidarUsuario(email, senha))
            {
                return RedirectToAction("Index", "PaginaInicial");
            }

            ViewBag.Mensagem = "Email ou senha inválidos.";
            return View("Index");
        }
    }
}