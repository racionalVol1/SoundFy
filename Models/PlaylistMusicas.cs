using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoundFy.Models
{
    public class PlaylistMusicas
    {
        
        public int PlaylistMusicaId { get; set; }
        
        [Display(Name = "Playlist")]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        [Display(Name = "Música")]
        public int MusicaId { get; set; }
        public Musica Musica { get; set; }
    }
}