using System.Data.SQLite;
using Data.Services;

namespace SoundFy.Data
{
    public class UsuarioRepository
    {
        private readonly string caminhoBanco = @"Data Source=C:\dev\SoundFy\SoundFy\Data\BancoDeDados\SoundFy.db";

        // Valida o usuário com email e senha
        public bool ValidarUsuario(string email, string senha)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha";
            using var cmd = new SQLiteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        // Verifica se o usuário já existe no banco de dados
        public bool ValidaUsuarioExistente(string email)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT * FROM Usuario WHERE Email = @Email";
            using var cmd = new SQLiteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

    //Registra um novo usuário no banco de dados
        public bool RegistrarUsuario(string email, string senha, string tipo)
        {
            try
            {
                using var conexao = new SQLiteConnection(caminhoBanco);
                conexao.Open();

                string insertSql = "INSERT INTO Usuario (Email, Senha, Tipo) VALUES (@Email, @Senha, @Tipo)";
                using var cmd = new SQLiteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);
                cmd.Parameters.AddWithValue("@Tipo", tipo);

                cmd.ExecuteNonQuery();

                new EmailServices().EnviarEmailConfirmacao(email);

                return true;
            }
            catch (SQLiteException ex)
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

        // Obtém o tipo de usuário baseado no email
        public string ObterTipoUsuario(string email)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Tipo FROM Usuario WHERE Email = @Email";
            using var cmd = new SQLiteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return reader["Tipo"].ToString();

            return null;
        }
    }
}
