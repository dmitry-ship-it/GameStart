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
        private readonly SmtpClient smtpClient;

        public EmailService(IOptions<MailSettings> settings, SmtpClient smtpClient)
        {
            this.settings = settings.Value;
            this.smtpClient = smtpClient;
        }

        public async Task SendMailAsync(EmailTemplate data, CancellationToken cancellationToken = default)
        {
            await smtpClient.ConnectAsync(
                host: settings.Host,
                port: settings.Port,
                options: ChooseSslOptions(),
                cancellationToken: cancellationToken);

            await smtpClient.AuthenticateAsync(settings.UserName, settings.Password, cancellationToken);
            await smtpClient.SendAsync(CreateMessageFromTemplate(data), cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);
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
            return settings switch
            {
                { UseSSL: true, UseStartTls: false } => SecureSocketOptions.SslOnConnect,
                { UseSSL: false, UseStartTls: true } => SecureSocketOptions.StartTls,
                _ => SecureSocketOptions.StartTlsWhenAvailable
            };
        }
    }
}
