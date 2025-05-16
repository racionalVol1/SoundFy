using Data.Services;
using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;


namespace SoundFy.Controllers
{
    public class LoginController() : Controller
    {

        private static Dictionary<string, int> tentativasLogin = new();

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
                tentativasLogin[email] = 0; 

                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";
                var navegador = Request.Headers["User-Agent"].ToString();

                var emailService = new EmailServices();
                emailService.EnviarEmailLogin(email, ip, navegador);

                return RedirectToAction("Index", "PaginaInicial");
            }
            else
            {               
                if (tentativasLogin.ContainsKey(email))
                    tentativasLogin[email]++;
                else
                    tentativasLogin[email] = 1;

                ViewBag.Mensagem = $"Email ou senha inválidos. Tentativas: {tentativasLogin[email]}";
                return View("Index");
            }
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