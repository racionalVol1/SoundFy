using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundFy.Models
{
    public class Musica
    {
        public int MusicaId { get; set; }
        
        [Display(Name = "Música")]
        public string NomeMusica { get; set; }

        [Display(Name = "Álbum")]
        [ForeignKey("Album")]
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public ICollection<PlaylistMusicas> PlaylistMusicas { get; set; } = new List<PlaylistMusicas>();        
    }
}