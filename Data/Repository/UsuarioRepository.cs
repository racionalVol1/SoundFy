using Microsoft.Data.Sqlite;
using Business.Utilities;
using Data.Config;
using Microsoft.Extensions.Configuration;
namespace SoundFy.Data

{
    public class UsuarioRepository
    {

        // Criação da instancia do EmailUltilities para enviar emails
        private readonly EmailUltilities emails = new EmailUltilities();       

        // Caminho do banco de dados
        private readonly string caminhoBanco;

        // Construtor para carregar a configuração do banco de dados
        public UsuarioRepository()
        {
            IConfigurationRoot config = ConfigHelper.LoadConfiguration();
            string caminhoArquivo = config.GetSection("DataSettings:ConexaoBanco").Value
                                    ?? throw new InvalidOperationException("ConexaoBanco não encontrada nas configurações.");

            caminhoBanco = $"Data Source={caminhoArquivo}";
        }

        // Valida o usuário com email e senha  
        public bool ValidarUsuario(string email, string senha)
        {
            using var conexao = new SqliteConnection(caminhoBanco);
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
            using var conexao = new SqliteConnection(caminhoBanco);
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
                using var conexao = new SqliteConnection(caminhoBanco);
                conexao.Open();

                string insertSql = "INSERT INTO Usuario (Email, Senha, Tipo) VALUES (@Email, @Senha, @Tipo)";
                using var cmd = new SqliteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);
                cmd.Parameters.AddWithValue("@Tipo", tipo);

                cmd.ExecuteNonQuery();

                new EmailUltilities().EnviarEmailConfirmacao(email);

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

        // Obtém o tipo de usuário baseado no email  
        public string? ObterTipoUsuario(string email)
        {
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Tipo FROM Usuario WHERE Email = @Email";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return reader["Tipo"].ToString();

            return null;
        }
    }
}
