using Data.BancoDeDados;
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class OuvinteRepository
    {       
        // Caminho do banco de dados
        private readonly string _caminhoBanco;

        public OuvinteRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }

        // Método para obter os bytes de uma música pelo ID
        public byte[]? ObterBytesMusicaPorId(int id)
        {
            using var conexao = new SqliteConnection( _caminhoBanco);

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