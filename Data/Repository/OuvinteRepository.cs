using Business.Utilities;
using Data.Config;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class OuvinteRepository
    {
        // Criação da instancia do EmailUltilities para enviar emails
        private readonly EmailUltilities emails = new EmailUltilities();

        // Caminho do banco de dados
        private readonly string caminhoBanco;

        // Construtor que carrega configurações do banco de dados
        public OuvinteRepository()
        {
            IConfigurationRoot config = ConfigHelper.LoadConfiguration();
            string caminhoArquivo = config.GetSection("DataSettings:ConexaoBanco").Value
                                    ?? throw new InvalidOperationException("ConexaoBanco não encontrada nas configurações.");

            caminhoBanco = $"Data Source={caminhoArquivo}";
        }

        // Método para obter os bytes de uma música pelo ID
        public byte[]? ObterBytesMusicaPorId(int id)
        {
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Arquivo FROM Musica WHERE Id = @Id";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader["Arquivo"] as byte[];
            }
            return null;
        }
    }
}