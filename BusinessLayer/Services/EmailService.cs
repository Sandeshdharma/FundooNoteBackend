using BusinessLayer.Config;
using BusinessLayer.IBusiness;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BusinessLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            this.smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmail(
            string toEmail,
            string subject,
            string body)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    smtpSettings.SenderName,
                    smtpSettings.SenderEmail));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            email.Body =
                new TextPart("html")
                {
                    Text = body
                };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                smtpSettings.Server,
                smtpSettings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                smtpSettings.Username,
                smtpSettings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}