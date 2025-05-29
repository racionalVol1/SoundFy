    using Data.Services;
    using Microsoft.AspNetCore.Mvc;
    using SoundFy.Data;
    using Microsoft.AspNetCore.Http;

    namespace SoundFy.Controllers
    {
        public class LoginController : Controller
        {

            //Criação de objetos
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            EmailServices emailServices = new EmailServices();

            // Gera um captcha de 6 dígitos e salva na sessão
            private void GerarCaptcha()
            {
                var random = new Random();
                string captcha = random.Next(100000, 999999).ToString();
                HttpContext.Session.SetString("CaptchaLogin", captcha);
                ViewBag.Captcha = captcha;
            }

            // Retorno de view da pagina de login
            public IActionResult Index()
            
        {
                GerarCaptcha();
                return View();
            }

            // Botão para atualizar o captcha
            public IActionResult AtualizarCaptcha()
            {
                GerarCaptcha();
                return RedirectToAction("Index");
            }

            // Autenticação do usuario       
            [HttpPost]
            public IActionResult Autenticar(string email, string senha, string captcha)
            {
                string? captchaCorreto = HttpContext.Session.GetString("CaptchaLogin");
                if (captcha != captchaCorreto)
                {
                    ViewBag.Mensagem = "Captcha incorreto.";
                    GerarCaptcha();
                    return View("Index");
                }

                if (usuarioRepository.ValidarUsuario(email, senha))
                {               
                    var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";
                    var navegador = Request.Headers["User-Agent"].ToString();
                                    
                    emailServices.EnviarEmailLogin(email, ip, navegador);

                    return RedirectToAction("Index", "PaginaInicial");
                }
                else
                {
                    ViewBag.Mensagem = "E-mail ou senha inválidos.";                
                    GerarCaptcha();
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
                if (usuarioRepository.ValidaUsuarioExistente(email))
                {                
                    emailServices.EnviarCodigoRecuperacao(email);
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