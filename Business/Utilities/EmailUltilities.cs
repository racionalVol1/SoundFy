using Microsoft.Data.Sqlite;
using System.Net.Mail;

namespace Business.Utilities
{
    public class EmailUltilities
    {
        //Caminho do banco de dados
        string caminhoBanco = $@"Data Source=""{Path.Combine(AppContext.BaseDirectory, "BancoDeDados", "SoundFy.db")}""";

        //Lista para armazenar códigos de recuperação
        private static readonly List<string> codigosRecuperacao = new List<string>();

        //Criar SMTP Client
        private SmtpClient CriarSmtpClient()
        {
            return new SmtpClient("localhost")
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        //Metodo para enviar email
        private void EnviarEmail(string destinatario, string assunto, string corpo)
        {
            try
            {
                using (var smtpClient = CriarSmtpClient())
                {
                    var mensagem = new MailMessage("nao-responda@soundfy.com", destinatario)
                    {
                        Subject = assunto,
                        Body = corpo
                    };
                    smtpClient.Send(mensagem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }

        //Metodo para confimar email ao logar
        public bool ConfirmarEmail(string email)
        {
            using (var conexao = new SqliteConnection(caminhoBanco))
            {
                conexao.Open();

                string sql = "UPDATE Usuario SET EmailConfirmado = 1 WHERE Email = @Email";
                using (var cmd = new SqliteCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        //Metodo para enviar email de confirmação ao criar conta
        public void EnviarEmailConfirmacao(string email)
        {
            string link = $"http://localhost:7105/Registro/ConfirmarEmail?email={email}";
            string corpo = $"Clique no link para confirmar seu e-mail: {link}";
            EnviarEmail(email, "Confirme seu e-mail", corpo);
        }

        //Metodo para enviar email de login
        public void EnviarEmailLogin(string email, string ip, string navegador)
        {
            string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string corpo = $"Seu login foi realizado com sucesso em {dataHora}.\nIP de acesso: {ip}\nNavegador: {navegador}";
            EnviarEmail(email, "Confirmação de Login - SoundFy", corpo);
        }

        //Metodo que gera um código de recuperação e envia para o email
        public string GerarCodigoRecuperacao()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Metodo que gera um código de recuperação e envia para o email
        public void EnviarCodigoRecuperacao(string email)
        {
            string codigo = GerarCodigoRecuperacao();
            codigosRecuperacao.Add(codigo);
            string corpo = $"Seu código de recuperação é: {codigo}";
            EnviarEmail(email, "Recuperação de Senha - SoundFy", corpo);
        }

        // Metodo pra validar codigo enviado
        public bool ValidarCodigoRecuperacao(string codigo)
        {
            if (codigosRecuperacao.Contains(codigo))
            {
                codigosRecuperacao.Remove(codigo);
                return true;
            }
            return false;
        }
    }
}
