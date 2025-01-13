using ApiLogin.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ApiLogin.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _useSsl;

        public EmailService(string smtpServer, int smtpPort, string smtpUsername,
                            string smtpPassword, bool useSsl = true)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _useSsl = useSsl;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = _useSsl;

                    await client.SendMailAsync("from@example.com", toEmail, subject, body);
                }
            }
            catch (Exception)
            {
                //TODO
            }
        }
    }
}