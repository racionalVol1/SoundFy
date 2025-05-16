using Data.Services;
using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class RegistroController() : Controller
    {

        UsuarioRepository usuarioRepository = new UsuarioRepository();

        EmailServices emailServices = new EmailServices();


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(string email, string senha)
        {
           

            if (usuarioRepository.ValidaUsuarioExistente(email))
            {
                ViewBag.Mensagem = "Usuario ja cadastrado.";
                return RedirectToAction("index", "login");
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
            
            var confirmado = emailServices.ConfirmarEmail(email, token);

            TempData["Mensagem"] = confirmado
                ? "E-mail confirmado com sucesso!"
                : "Erro ao confirmar e-mail.";

            return RedirectToAction("Index", "Login");
        }

    }
}