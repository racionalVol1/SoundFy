using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Utilities;
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class OuvinteRepository
    {
        EmailUltilities emails = new EmailUltilities();
        private readonly string caminhoBanco;

        public bool ReproduzirMusica(string titulo)
        {
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT * FROM Usuario WHERE Titulo = @Titulo";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Titulo", titulo);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }        
    }
}