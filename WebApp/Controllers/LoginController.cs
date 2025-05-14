using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;


namespace SoundFy.Controllers
{
    public class LoginController() : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Autenticar(string email, string senha)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            if (usuarioRepository.ValidarUsuario(email, senha))
            {
                return RedirectToAction("Index", "PaginaInicial");
            }

            ViewBag.Mensagem = "Email ou senha inválidos.";
            return View("Index");
        }               
    }
}