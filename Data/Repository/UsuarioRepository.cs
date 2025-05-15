using System.Data.SQLite;
using System.Net.Mail;

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
                EnviarEmailConfirmacao(email, token); // Envia e-mail após registrar
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar usuário: {ex.Message}");
                return false;
            }
        }

        public bool ConfirmarEmail(string email, string token)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();

            string sql = "UPDATE Usuario SET EmailConfirmado = 1 WHERE Email = @Email AND TokenConfirmacao = @Token";

            using var cmd = new SQLiteCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Token", token);

            return cmd.ExecuteNonQuery() > 0;
        }

        public void EnviarEmailConfirmacao(string email, string token)
        {
            var smtpClient = new SmtpClient("localhost") // FakeSMTP
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            string link = $"http://localhost:7105/Registro/ConfirmarEmail?email={email}&token={token}";

            var mensagem = new MailMessage("nao-responda@soundfy.com", email)
            {
                Subject = "Confirme seu e-mail",
                Body = $"Clique no link para confirmar seu e-mail: {link}"
            };

            smtpClient.Send(mensagem);
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

    }
}
