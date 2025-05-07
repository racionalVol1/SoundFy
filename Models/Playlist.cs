using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoundFy.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }

        [Display(Name = "Playlist")]
        public string NomePlaylist { get; set; }     
        
        public ICollection<PlaylistMusicas> PlaylistMusicas { get; set; } = new List<PlaylistMusicas>();

    }
}