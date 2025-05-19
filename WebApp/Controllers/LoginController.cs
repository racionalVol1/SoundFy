using Data.Services;
using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class LoginController() : Controller
    {
        private static Dictionary<string, int> tentativasLogin = new();

        //Retorno de view da pagina de login
        public IActionResult Index()
        {
            return View();
        }

        //Autenticação do usuario
        [HttpPost]
        public IActionResult Autenticar(string email, string senha, string? captcha)
        {
            if (tentativasLogin.ContainsKey(email) && tentativasLogin[email] >= 3)
            {
                if (string.IsNullOrEmpty(captcha) || captcha != "1234")
                {
                    ViewBag.ExibirCaptcha = true;
                    ViewBag.Mensagem = "Por favor, resolva o CAPTCHA para continuar.";
                    return View("Index");
                }
            }

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

                if (tentativasLogin[email] >= 3)
                    ViewBag.ExibirCaptcha = true;

                ViewBag.Mensagem = $"Email ou senha inválidos. Tentativas: {tentativasLogin[email]}";
                return View("Index");
            }
        }

        //Retorno de view a pagina de recuperar senha
        public IActionResult RecuperarSenha()
        {
            return View("RecuperarSenha");
        }      
    }
}