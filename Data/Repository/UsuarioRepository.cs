using System.Data.SQLite;
using Data.Services;

namespace SoundFy.Data
{
    public class UsuarioRepository
    {
        string caminhoBanco = $@"Data Source=""{Path.Combine(AppContext.BaseDirectory, "BancoDeDados", "SoundFy.db")}""";

        public bool ValidarUsuario(string email, string senha)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();
            Console.WriteLine("Conexão com SQLite aberta com sucesso!");

            string selectSql = "SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha";

            using var cmd = new SQLiteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        public bool RegistrarUsuario(string email, string senha)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();
            Console.WriteLine("Conexão com SQLite aberta com sucesso!");

            string token = Guid.NewGuid().ToString();

            string insertSql = "INSERT INTO Usuario (Email, Senha, EmailConfirmado, TokenConfirmacao) VALUES (@Email, @Senha, 0, @Token)";

            using var cmd = new SQLiteCommand(insertSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);
            cmd.Parameters.AddWithValue("@Token", token);

            try
            {
                cmd.ExecuteNonQuery();
                
                var emailService = new EmailServices();
                emailService.EnviarEmailConfirmacao(email, token);

                return true;
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Erro: E-mail já cadastrado.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar usuário: {ex.Message}");
                return false;
            }
        }

        public bool ValidaUsuarioExistente(string email)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();
            Console.WriteLine("Conexão com SQLite aberta com sucesso!");

            string selectSql = "SELECT * FROM Usuario WHERE Email = @Email";

            using var cmd = new SQLiteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }


        public bool AlterarSenha(string email, string senha)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();

            string updateSql = "UPDATE Usuario SET Senha = @Senha WHERE Email = @Email";

            using var cmd = new SQLiteCommand(updateSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
