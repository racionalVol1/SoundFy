using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoundFy.Models
{
    public class Album
    {
        public int AlbumId { get; set; }

        [Display(Name = "Álbum")]
        public string NomeAlbum { get; set; }
        
        [Display(Name = "Artista")]
        public string User { get; set; } 

        public ICollection<Musica> Musicas { get; set; }
    }
}