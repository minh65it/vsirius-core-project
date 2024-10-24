using System.Net.Mail;
using System.Net;

namespace IdentityApi.Helpers.Utilities
{
    public interface IEmailSenderSmtp
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<bool> CheckEmailExists(string email, string subject, string message);
    }
    public class EmailSenderSmtp : IEmailSenderSmtp
    {
        private readonly IConfiguration _configuration;

        public EmailSenderSmtp(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            EmailSenderOptions _options = _configuration.GetSection("EmailSenderOptions").Get<EmailSenderOptions>();
            var client = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
            {
                Credentials = new NetworkCredential(_options.SmtpUsername, _options.SmtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_options.FromEmail, email, subject, message) { IsBodyHtml = true };

            return client.SendMailAsync(mailMessage);
        }
        public async Task<bool> CheckEmailExists(string email, string subject, string message)
        {
            MailAddress mailAddress = new MailAddress(email);

            EmailSenderOptions _options = _configuration.GetSection("EmailSenderOptions").Get<EmailSenderOptions>();
                var client = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
                {
                    Credentials = new NetworkCredential(_options.SmtpUsername, _options.SmtpPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage(_options.FromEmail, email, subject, message) { IsBodyHtml = true };

            try
            {
                await client.SendMailAsync(mailMessage);
                return true;

            }
            catch (FormatException ex)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public class EmailSenderOptions
        {
            public string FromEmail { get; set; }
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpUsername { get; set; }
            public string SmtpPassword { get; set; }
        }
    }
}
