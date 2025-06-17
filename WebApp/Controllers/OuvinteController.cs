using Data.Models; // Adicione esta linha para ter acesso ao MusicaModel
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Adicione esta linha para usar FirstOrDefault

namespace WebApp.Controllers
{
    public class OuvinteController : Controller
    {
        ArtistaRepository artistaRepository = new ArtistaRepository();
        OuvinteRepository ouvinteRepository = new OuvinteRepository();

        // Pagina de erro
        public IActionResult Erro()
        {
            return View("Erro");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicas = artistaRepository.ListarMusicas();
            return View(musicas);
        }

        // Esta ação agora vai CARREGAR A PÁGINA com os detalhes da música e o player
        public IActionResult Reproduzir(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login"); // Proteção de rota

            var musicas = artistaRepository.ListarMusicas();
            var musica = musicas.FirstOrDefault(m => m.Id == id);

            if (musica == null)
            {
                return NotFound(); // Retorna 404 se a música não for encontrada
            }

            // Retorna a View 'Reproduzir.cshtml' e passa o objeto 'musica' como Model
            return View(musica);
        }

        // Esta nova ação será chamada pelo <audio> na sua View para obter o stream de áudio
        public IActionResult StreamAudio(int id)
        {
            // Opcional: Adicione a verificação de login aqui também, se desejar proteger o stream
            // if (HttpContext.Session.GetString("logado") != "true")
            //     return Unauthorized(); // Ou outro tipo de retorno

            byte[]? audioBytes = ouvinteRepository.ObterBytesMusicaPorId(id);
            if (audioBytes == null)
            {
                return NotFound(); // Retorna 404 se o arquivo de áudio não for encontrado
            }

            // Retorna os bytes do áudio com o tipo MIME correto
            return File(audioBytes, "audio/mpeg");
        }
    }
}
