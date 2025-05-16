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

        string caminhoBanco = $@"Data Source=""{Path.Combine(AppContext.BaseDirectory, "BancoDeDados", "SoundFy.db")}""";

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
            var smtpClient = new SmtpClient("localhost")
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

        public void EnviarEmailLogin(string email, string ip, string navegador)
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
    }
}