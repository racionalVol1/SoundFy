using System.Data.SQLite;

namespace SoundFy.Data
{
    public class UsuarioRepository
    {        
        public bool ValidarUsuario(string email, string senha)
        {
            string caminhoBanco = "Data Source=Data/SoundFy.db";

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

        public bool RegistrarUsuario( string email, string senha)
        {
            string caminhoBanco = "Data Source=Data/SoundFy.db";

            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();
            Console.WriteLine("Conexão com SQLite aberta com sucesso!");

            string insertSql = "INSERT INTO Usuario (Email, Senha) VALUES (@Email, @Senha)";

            using var cmd = new SQLiteCommand(insertSql, conexao);          
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            try
            {
                cmd.ExecuteNonQuery();
                return true;  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar usuário: {ex.Message}");
                return false;  
            }
        }
    }
}