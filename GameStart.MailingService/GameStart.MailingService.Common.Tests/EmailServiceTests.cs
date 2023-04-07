using GameStart.MailingService.Common.Services;
using GameStart.MailingService.Common.Settings;
using GameStart.Shared.MessageBus.Models.EmailModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GameStart.MailingService.Common.Tests
{
    public class EmailServiceTests
    {
        [Fact]
        public async Task SendMailAsync_ShouldSendMimeMessage()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var mailSettings = new MailSettings
            {
                DisplayName = "Test",
                From = "Test",
                UseSSL = false,
                UseStartTls = true,
                Host = "smtp.test.com",
                UserName = "test@mail.com",
                Password = "Pa$$w0rd",
                Port = 587
            };

            var smtpClientMock = Substitute.For<SmtpClient>();
            var optionsMock = Substitute.For<IOptions<MailSettings>>();
            optionsMock.Value.Returns(mailSettings);

            var emailTemplate = new EmailTemplate
            {
                To = "some.user@mail.com",
                Subject = "Test",
                Body = "Lorem ipsum"
            };

            var sut = new EmailService(optionsMock, smtpClientMock);

            // Act
            await sut.SendMailAsync(emailTemplate, cancellationToken);

            // Assert
            await smtpClientMock.Received().SendAsync(Arg.Any<MimeMessage>(), cancellationToken);
        }
    }
}
