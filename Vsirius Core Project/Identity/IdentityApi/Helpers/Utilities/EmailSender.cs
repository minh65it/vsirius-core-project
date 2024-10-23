using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace IdentityApi.Helpers.Utilities
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<bool> CheckEmailExists(string email, string subject, string message);
        Task<bool> SendEmailSendgird(string email, string subject, string message);
    }
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmailSendgird(string toEmail, string subject, string message)
        {
            var apiKey = _configuration["SendGirdEmail:APIKey"];
            var client = new SendGridClient(apiKey);
            var fromEmail = _configuration["SendGirdEmail:From"];
            var fromName = _configuration["SendGirdEmail:FromName"];
            var from = new EmailAddress(fromEmail, fromName);
            var toNameCustomer = _configuration["SendGirdEmail:ToNameCustomer"];
            var to = new EmailAddress(toEmail, toNameCustomer);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, message);
            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
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
