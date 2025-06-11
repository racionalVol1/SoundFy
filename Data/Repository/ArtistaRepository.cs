using Business.Utilities;
using Data.Config;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Data.Models;


namespace Data.Repository
{
    public class ArtistaRepository
    {

        // Criação da instancia do EmailUltilities para enviar emails
        private readonly EmailUltilities emails = new EmailUltilities();

        // Caminho do banco de dados
        private readonly string caminhoBanco;

        // Construtor que carrega configurações do banco de dados
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

        //Listar musicas
        public List<MusicaModel> ListarMusicas()
        {
            var musicas = new List<MusicaModel>();
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Id, Titulo, NomeArtista, Genero, Ano, NomeArquivo FROM Musica";
            using var cmd = new SqliteCommand(selectSql, conexao);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                musicas.Add(new MusicaModel
                {
                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                    Titulo = reader["Titulo"]?.ToString() ?? string.Empty,
                    NomeArtista = reader["NomeArtista"]?.ToString() ?? string.Empty,
                    Genero = reader["Genero"]?.ToString() ?? string.Empty,
                    Ano = reader["Ano"] != DBNull.Value ? Convert.ToInt32(reader["Ano"]) : 0,
                    NomeArquivo = reader["NomeArquivo"]?.ToString() ?? string.Empty
                });
            }
            return musicas;
        }

        //Verificar musicas duplicadas
        public bool VerificarMusicaDuplicada(string titulo, string nomeArtista, string genero, int ano, string nomeArquivo, byte[] arquivo)
        {
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT COUNT(*) FROM Musica WHERE Titulo = @Titulo AND NomeArtista = @NomeArtista " +
                               "AND Genero = @Genero AND Ano = @Ano AND NomeArquivo = @NomeArquivo";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Titulo", titulo);
            cmd.Parameters.AddWithValue("@NomeArtista", nomeArtista);
            cmd.Parameters.AddWithValue("@Genero", genero);
            cmd.Parameters.AddWithValue("@Ano", ano);
            cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        //Metodo para excluir musicas
        public bool ExcluirMusicaPorId(int id)
        {
            try
            {
                using var conexao = new SqliteConnection(caminhoBanco);
                conexao.Open();

                string deleteSql = "DELETE FROM Musica WHERE Id = @Id";
                using var cmd = new SqliteCommand(deleteSql, conexao);
                cmd.Parameters.AddWithValue("@Id", id);
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
    }
}