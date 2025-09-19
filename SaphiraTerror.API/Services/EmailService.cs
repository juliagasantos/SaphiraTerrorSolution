using SaphiraTerror.API.DTOs;
using SaphiraTerror.API.Interfaces;
using System.Net;
using System.Net.Mail;

namespace EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        //config de injeção
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Enviar (EmailDTO email)
        {
            //cria o email
            MailMessage message = new MailMessage();

            //remetente
            message.From = new MailAddress(email.EmailRemetente, email.NomeRemetente);

            //destinatario
            message.To.Add(new MailAddress(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:FromName"]));

            //assunto
            message.Subject = email.Assunto;

            //corpo do email
            message.Body = email.Mensagem;

            //configurar o SMTPClient
            SmtpClient smtp = new SmtpClient();

            //dadod do servidor (gmail)
            smtp.Host = _configuration["EmailSettings:SmtpHost"];
            smtp.Port = Convert.ToInt32(_configuration["EmailSettings:SmtpPort"]);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_configuration["EmailSettings:EmailUser"], _configuration["EmailSettings:EmailPassword"]);

            //envia email
            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar e-mail: " + ex.Message);
            }
        }
    }
}
