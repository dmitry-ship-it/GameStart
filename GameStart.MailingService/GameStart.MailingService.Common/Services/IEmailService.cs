using GameStart.Shared.MessageBus.Models.EmailModels;

namespace GameStart.MailingService.Common.Services
{
    public interface IEmailService
    {
        Task SendMailAsync(EmailTemplate data, CancellationToken cancellationToken = default);
    }
}
