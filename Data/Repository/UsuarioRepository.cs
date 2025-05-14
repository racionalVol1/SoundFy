using System.Data.SQLite;

namespace SoundFy.Data
{
    public class UsuarioRepository
    {
        string caminhoBanco = $@"Data Source=""{Path.Combine(AppContext.BaseDirectory, "BancoDeDados", "SoundFy.db")}""";

        public bool ValidarUsuario(string email, string senha)
        {
            try
            {
                using var conexao = new SQLiteConnection(caminhoBanco);
                conexao.Open();
                Console.WriteLine("Conexão com banco de dados realizada com sucesso!");

                string selectSql = "SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha";

                using var cmd = new SQLiteCommand(selectSql, conexao);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);

                using var reader = cmd.ExecuteReader();
                return reader.Read();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao acessar banco de dados: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação invalida: {ex.Message}");
            }           
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao validar usuario: {ex.Message}");
            }

            return false;
        }

        public bool RegistrarUsuario(string email, string senha)
        {
            try
            {
                using var conexao = new SQLiteConnection(caminhoBanco);
                conexao.Open();
                Console.WriteLine("Conexão com banco de dados realizada com sucesso!");

                string insertSql = "INSERT INTO Usuario (Email, Senha) VALUES (@Email, @Senha)";

                using var cmd = new SQLiteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao acessar banco de dados: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação invalida: {ex.Message}");
            }           
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar usuario: {ex.Message}");
            }

            return false;
        }
    }
}
