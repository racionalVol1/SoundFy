using Business.Utilities;
using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class RegistroController : Controller
    {
        //Criação de objetos
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        EmailUltilities emails = new EmailUltilities();

        // Pagina de erro
        public IActionResult Erro()
        {
            return View("Erro");
        }

        // Método para retornar a view de registro
        public IActionResult Index()
        {
            return View();
        }

        // Método para registrar um novo usuário
        [HttpPost]
        public IActionResult Registrar(string email, string senha, string tipo)
        {
            if (usuarioRepository.ValidaUsuarioExistente(email))
            {
                ViewBag.Mensagem = "Usuário já cadastrado.";
                return View("Index");
            }

            if (usuarioRepository.RegistrarUsuario(email, senha, tipo))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Mensagem = "Erro ao registrar usuário.";
            return View("Index");
        }

        // Método para enviar o e-mail de confirmação
        [HttpGet]
        public IActionResult ConfirmarEmail(string email)
        {
            var confirmado = emails.ConfirmarEmail(email);

            TempData["Mensagem"] = confirmado
                ? "E-mail confirmado com sucesso!"
                : "Erro ao confirmar e-mail.";

            return RedirectToAction("Index", "Login");
        }
    }
}
