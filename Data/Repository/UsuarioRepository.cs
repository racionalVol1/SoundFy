using Data.BancoDeDados;
using Data.Models;
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class UsuarioRepository
    {
        // Caminho do banco de dados
        private readonly string _caminhoBanco;

        public UsuarioRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }

        // Valida o usuário com email e senha  
        public bool ValidarUsuario(string email, string senha)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        // Verifica se o usuário já existe no banco de dados  
        public bool ValidaUsuarioExistente(string email)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT * FROM Usuario WHERE Email = @Email";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        // Registra um novo usuário no banco de dados  
        public bool RegistrarUsuario(string email, string senha, string tipo)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string insertSql = "INSERT INTO Usuario (Email, Senha, Tipo) VALUES (@Email, @Senha, @Tipo)";
                using var cmd = new SqliteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);
                cmd.Parameters.AddWithValue("@Tipo", tipo);

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return false;
            }

        }

        // Obtém o tipo de usuário baseado no email  
        public string? ObterTipoUsuario(string email)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Tipo FROM Usuario WHERE Email = @Email";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return reader["Tipo"].ToString();

            return null;
        }

        // Obtém o usuário completo baseado no email  
        public UsuarioModel? ObterUsuarioPorEmail(string email)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Id, Email, Senha, Tipo FROM Usuario WHERE Email = @Email";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UsuarioModel
                {
                    Id = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    Senha = reader.GetString(2),
                    Tipo = reader.GetString(3)
                };
            }
            return null;
        }
    }
}
