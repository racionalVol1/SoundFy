using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class RegistroController() : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(string email, string senha)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            if (usuarioRepository.ValidaUsuarioExistente(email))
            {
                return RedirectToAction("index", "login");
                //ViewBag.Mensagem = "Usuario ja cadastrado.";
                //return View("Index");
            }
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
        public IActionResult ConfirmarEmail(string email, string token)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            bool confirmado = usuarioRepository.ConfirmarEmail(email, token);

            TempData["Mensagem"] = confirmado
                ? "E-mail confirmado com sucesso!"
                : "Erro ao confirmar e-mail.";

            return RedirectToAction("Index", "Login");
        }       
    }
}