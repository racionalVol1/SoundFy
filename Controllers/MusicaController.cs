using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoundFy.Data;
using SoundFy.Models;

namespace SoundFy
{
    public class MusicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Musica
        public async Task<IActionResult> Index()
        {
            var musicas = _context.Musicas.Include(m => m.Album);
            return View(await musicas.ToListAsync());
        }

        // GET: Musica/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musica = await _context.Musicas
                .Include(m => m.Album)
                .FirstOrDefaultAsync(m => m.MusicaId == id);
            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }

        // GET: Musica/Create

        [HttpGet]
        public async Task<IActionResult> Create(int id) // id = AlbumId
        {
            var album = await _context.Albuns.FindAsync(id);
            if (album == null) return NotFound();

            ViewBag.AlbumId = album.AlbumId;
            ViewBag.AlbumNome = album.NomeAlbum;

            return View();
        }

        // POST: Musica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<string> NomeMusica, int AlbumId)
        {
            if (NomeMusica == null || NomeMusica.Count == 0)
            {
                ModelState.AddModelError("", "Adicione pelo menos uma música.");
            }

            if (ModelState.IsValid)
            {
                foreach (var nome in NomeMusica)
                {
                    var musica = new Musica
                    {
                        NomeMusica = nome,
                        AlbumId = AlbumId
                    };
                    _context.Musicas.Add(musica);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Album", new { id = AlbumId });
            }

            var album = await _context.Albuns.FindAsync(AlbumId);
            ViewBag.AlbumId = AlbumId;
            ViewBag.AlbumNome = album?.NomeAlbum;

            return View();
        }
        // GET: Musica/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musica = await _context.Musicas.FindAsync(id);
            if (musica == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Albuns, "AlbumId", "AlbumId", musica.AlbumId);
            return View(musica);
        }

        // POST: Musica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusicaId,NomeMusica,AlbumId")] Musica musica)
        {
            if (id != musica.MusicaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicaExists(musica.MusicaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Albuns, "AlbumId", "AlbumId", musica.AlbumId);
            return View(musica);
        }

        // GET: Musica/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musica = await _context.Musicas
                .Include(m => m.Album)
                .FirstOrDefaultAsync(m => m.MusicaId == id);
            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }

        // POST: Musica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musica = await _context.Musicas.FindAsync(id);
            if (musica == null)
            {
                return NotFound();
            }

            _context.Musicas.Remove(musica);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Album", new { id = musica.AlbumId }); // Redireciona para a edição do álbum
        }

        private bool MusicaExists(int id)
        {
            return _context.Musicas.Any(e => e.MusicaId == id);
        }       
    }
}
