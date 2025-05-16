using Data.Services;
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
                var emailService = new EmailServices();
                emailService.EnviarEmailLogin(email);

                return RedirectToAction("Index", "PaginaInicial");
            }

            ViewBag.Mensagem = "Email ou senha inválidos.";
            return View("Index");
        }

        public IActionResult RecuperarSenha()
        {
            return View("RecuperarSenha");
        }

        [HttpPost]
        public IActionResult AlterarSenha(string email, string senha, string confirmarSenha)
        {
            if (senha != confirmarSenha)
            {
                ViewBag.Mensagem = "As senhas não coincidem.";
                return View("TrocarSenha");
            }

            UsuarioRepository usuarioRepository = new UsuarioRepository();

            if (usuarioRepository.AlterarSenha(email, senha))
            {
                TempData["Mensagem"] = "Senha alterada com sucesso. Faça login com sua nova senha.";
                return RedirectToAction("Login");
            }

            ViewBag.Mensagem = "Erro ao alterar a senha.";
            return View("TrocarSenha");
        }
    }
}