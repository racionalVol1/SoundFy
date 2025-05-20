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
        public IActionResult RecuperarConta()
        {
            return View("RecuperarConta");
        }

        //Validação de email para recuperar conta
        [HttpPost]
        public IActionResult RecuperarConta(string email)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            if (usuarioRepository.ValidaUsuarioExistente(email))
            {
                var emailService = new EmailServices();
                emailService.EnviarCodigoRecuperacao(email);
                ViewBag.Mensagem = "Um e-mail de recuperação foi enviado para você.";
            }
            else
            {
                ViewBag.Mensagem = "E-mail não encontrado.";
            }
            return View("ValidarCodigo");
        }

        //Retorno de view a pagina validar codigo
        public IActionResult ValidarCodigo()
        {
            return View("ValidarCodigo");
        }

        //Validação do código de recuperação
        [HttpPost]
        public IActionResult ValidarCodigo(string codigo)
        {
            var emailService = new EmailServices();
            if (emailService.ValidarCodigoRecuperacao(codigo))
            {
                return RedirectToAction("Index", "PaginaInicial");
            }
            else
            {
                ViewBag.Mensagem = "Código inválido.";
                return View("ValidarCodigo");
            }
        }
    }
}