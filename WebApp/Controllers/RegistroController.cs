using Data.Services;
using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class RegistroController : Controller
    {
        //Criação de objetos
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        EmailServices emailServices = new EmailServices();

        //Retorno de view da pagina de registro
        public IActionResult Index()
        {
            return View();
        }

        //Metodo para registrar usuario
        [HttpPost]
        public IActionResult Registrar(string email, string senha)
        {


            if (usuarioRepository.RegistrarUsuario(email, senha))
            {
                return RedirectToAction("Index", "login");
            }
            if (usuarioRepository.ValidaUsuarioExistente(email))
            {
                ViewBag.Mensagem = "Usuario ja cadastrado.";
                return RedirectToAction("index", "login");
            }
            else
            {
                ViewBag.Mensagem = "Erro ao registrar usuário.";
                return View("Index");
            }
        }

        //Metodo para enviar email de confirmação
        [HttpGet]
        public IActionResult ConfirmarEmail(string email)
        {

            var confirmado = emailServices.ConfirmarEmail(email);

            TempData["Mensagem"] = confirmado
                ? "E-mail confirmado com sucesso!"
                : "Erro ao confirmar e-mail.";

            return RedirectToAction("Index", "Login");
        }

    }
}