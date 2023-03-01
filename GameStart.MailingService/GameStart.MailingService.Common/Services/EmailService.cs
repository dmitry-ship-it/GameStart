using GameStart.MailingService.Common.Settings;
using GameStart.Shared.MessageBus.Models.EmailModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GameStart.MailingService.Common.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings settings;

        public EmailService(IOptions<MailSettings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task SendMailAsync(EmailTemplate data, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                host: settings.Host,
                port: settings.Port,
                options: ChooseSslOptions(),
                cancellationToken: cancellationToken);

            await smtp.AuthenticateAsync(settings.UserName, settings.Password, cancellationToken);
            await smtp.SendAsync(CreateMessageFromTemplate(data), cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }

        private MimeMessage CreateMessageFromTemplate(EmailTemplate data)
        {
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            // Sender
            mail.From.Add(new MailboxAddress(settings.DisplayName, settings.From));
            mail.Sender = new MailboxAddress(settings.DisplayName, settings.From);

            // Receiver
            mail.To.Add(MailboxAddress.Parse(data.To));

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = data.Subject;
            body.HtmlBody = data.Body;
            mail.Body = body.ToMessageBody();

            return mail;
        }

        private SecureSocketOptions ChooseSslOptions()
        {
            if (settings.UseSSL)
            {
                return SecureSocketOptions.SslOnConnect;
            }
            else if (settings.UseStartTls)
            {
                return SecureSocketOptions.StartTls;
            }
            else
            {
                return SecureSocketOptions.StartTlsWhenAvailable;
            }
        }
    }
}
