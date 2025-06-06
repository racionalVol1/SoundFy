using Business.Utilities;
using Data.Config;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Data.Models;


namespace Data.Repository
{
    public class ArtistaRepository
    {

        EmailUltilities emails = new EmailUltilities();
        private readonly string caminhoBanco;

        public ArtistaRepository()
        {
            IConfigurationRoot config = ConfigHelper.LoadConfiguration();
            string caminhoArquivo = config.GetSection("DataSettings:ConexaoBanco").Value
                                    ?? throw new InvalidOperationException("ConexaoBanco não encontrada nas configurações.");

            caminhoBanco = $"Data Source={caminhoArquivo}";
        }

        //Metodo para adcionar musicas
        public bool AdicionarMusica(string titulo, string nomeArtista, string genero, int ano, string nomeArquivo, byte[] arquivo)
        {
            try
            {
                using var conexao = new SqliteConnection(caminhoBanco);
                conexao.Open();

                string insertSql = "INSERT INTO Musica (Titulo, NomeArtista, Genero, Ano, NomeArquivo, Arquivo) " +
                                   "VALUES (@Titulo, @NomeArtista, @Genero, @Ano, @NomeArquivo, @Arquivo)";
                using var cmd = new SqliteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@NomeArtista", nomeArtista);
                cmd.Parameters.AddWithValue("@Genero", genero);
                cmd.Parameters.AddWithValue("@Ano", ano);
                cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);
                cmd.Parameters.AddWithValue("@Arquivo", arquivo ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro SQLite: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return false;
            }
        }

        public List<MusicaModel> ListarMusicas()
        {
            var musicas = new List<MusicaModel>();
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Titulo, NomeArtista, Genero, Ano, NomeArquivo FROM Musica";
            using var cmd = new SqliteCommand(selectSql, conexao);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                musicas.Add(new MusicaModel
                {
                    Titulo = reader["Titulo"]?.ToString() ?? string.Empty,
                    NomeArtista = reader["NomeArtista"]?.ToString() ?? string.Empty,
                    Genero = reader["Genero"]?.ToString() ?? string.Empty,
                    Ano = reader["Ano"] != DBNull.Value ? Convert.ToInt32(reader["Ano"]) : 0,
                    NomeArquivo = reader["NomeArquivo"]?.ToString() ?? string.Empty
                });
            }
            return musicas;
        }
    }
}