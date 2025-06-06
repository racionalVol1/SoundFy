using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class MusicaModel
    {
        public required string Titulo { get; set; }
        public required string NomeArtista { get; set; }
        public required string Genero { get; set; }
        public int Ano { get; set; }
        public required string NomeArquivo { get; set; }
    }
}