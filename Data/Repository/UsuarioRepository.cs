using System.Data.SQLite;
using Data.Services;

namespace SoundFy.Data
{
    public class UsuarioRepository
    {
        //Caminho do banco de dados
        string caminhoBanco = $@"Data Source=""{Path.Combine(AppContext.BaseDirectory, "BancoDeDados", "SoundFy.db")}""";

        //Metodo para validar o usuario
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
        
        //Metodo para validar se usario esta cadastrado
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

        //Metodo para registrar o usuario e adcionar no DB
        public bool RegistrarUsuario(string email, string senha)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();
            Console.WriteLine("Conexão com SQLite aberta com sucesso!");

            string token = Guid.NewGuid().ToString();

            string insertSql = "INSERT INTO Usuario (Email, Senha) VALUES (@Email, @Senha)";

            using var cmd = new SQLiteCommand(insertSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);          

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
        
    }
}
