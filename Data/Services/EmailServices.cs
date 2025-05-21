using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Data.Services
{
    public class EmailServices
    {
        //Caminho do banco de dados
        string caminhoBanco = $@"Data Source=""{Path.Combine(AppContext.BaseDirectory, "BancoDeDados", "SoundFy.db")}""";

        //Metodo para confimar email ao logar
        public bool ConfirmarEmail(string email)
        {
            using var conexao = new SQLiteConnection(caminhoBanco);
            conexao.Open();

            string sql = "UPDATE Usuario SET EmailConfirmado = 1 WHERE Email = @Email";

            using var cmd = new SQLiteCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);            

            return cmd.ExecuteNonQuery() > 0;
        }

        //Metodo para enviar email de confirmação ao criar conta
        public void EnviarEmailConfirmacao(string email)
        {
            var smtpClient = new SmtpClient("localhost")
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            string link = $"http://localhost:7105/Registro/ConfirmarEmail?email={email}";

            var mensagem = new MailMessage("nao-responda@soundfy.com", email)
            {
                Subject = "Confirme seu e-mail",
                Body = $"Clique no link para confirmar seu e-mail: {link}"
            };

            smtpClient.Send(mensagem);
        }

        //Metodo para enviar email de login
        public void EnviarEmailLogin(string email, string ip, string navegador)
        {           

            try
            {
                var smtpClient = new SmtpClient("localhost")
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string corpo = $"Seu login foi realizado com sucesso em {dataHora}.\n" +
                               $"IP de acesso: {ip}\n" +
                               $"Navegador: {navegador}";

                var mensagem = new MailMessage("nao-responda@soundfy.com", email)
                {
                    Subject = "Confirmação de Login - SoundFy",
                    Body = corpo
                };

                smtpClient.Send(mensagem);

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }            
        }

        //Metodo que gera um código de recuperação e envia para o email
        public void EnviarCodigoRecuperaçao(string email)
        {
            var smtpClient = new SmtpClient("localhost")
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var GerarCodigoRecuperacao = () =>
            {
                Random random = new Random();
                string codigo = random.Next(100000, 999999).ToString();
                return $"Seu código de recuperação é: {codigo}";
            };

            string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string corpo = GerarCodigoRecuperacao();

            var mensagem = new MailMessage("nao-responda@soundfy.com", email)
            {
                Subject = "Confirmação de Login - SoundFy",
                Body = corpo
            };

            smtpClient.Send(mensagem);
        }

        //Metodo pra validar codigo enviado
        private static readonly List<string> codigosRecuperacao = new List<string>();

        // Metodo que gera um código de recuperação e envia para o email
        public void EnviarCodigoRecuperacao(string email)
        {
            var smtpClient = new SmtpClient("localhost")
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            Random random = new Random();
            string codigo = random.Next(100000, 999999).ToString();

            codigosRecuperacao.Add(codigo);

            string corpo = $"Seu código de recuperação é: {codigo}";

            var mensagem = new MailMessage("nao-responda@soundfy.com", email)
            {
                Subject = "Recuperação de Senha - SoundFy",
                Body = corpo
            };

            smtpClient.Send(mensagem);
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