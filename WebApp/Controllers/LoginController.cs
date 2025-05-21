using Data.Services;
using Microsoft.AspNetCore.Mvc;
using SoundFy.Data;

namespace SoundFy.Controllers
{
    public class LoginController() : Controller
    {

        //Criação de objetos
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        EmailServices emailServices = new EmailServices();

        //Retorno de view da pagina de login
        public IActionResult Index()
        {
            return View();
        }

        //Autenticação do usuario
        [HttpPost]
        public IActionResult Autenticar(string email, string senha, string captcha = "")
        {
            int tentativas = HttpContext.Session.GetInt32("Tentativas") ?? 0;
            string? codigoSalvo = HttpContext.Session.GetString("CaptchaCodigo");

            if (tentativas >= 3)
            {
                if (string.IsNullOrEmpty(captcha) || captcha != codigoSalvo)
                {
                    ViewBag.Mensagem = "Captcha incorreto.";
                    ViewBag.ExibirCaptcha = true;
                    ViewBag.ValorCaptcha = codigoSalvo; 
                    return View("Index");
                }
            }

            if (usuarioRepository.ValidarUsuario(email, senha))
            {
                HttpContext.Session.Remove("Tentativas");
                HttpContext.Session.Remove("CaptchaCodigo");
                return RedirectToAction("Index", "PaginaInicial");
            }

            
            tentativas++;
            HttpContext.Session.SetInt32("Tentativas", tentativas);

            if (tentativas >= 3)
            {
                string novoCodigo = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("CaptchaCodigo", novoCodigo);
                ViewBag.ExibirCaptcha = true;
                ViewBag.ValorCaptcha = novoCodigo;
            }

            ViewBag.Mensagem = "Email ou senha incorretos.";
            return View("Index");
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

            if (emailServices.ValidarCodigoRecuperacao(codigo))
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