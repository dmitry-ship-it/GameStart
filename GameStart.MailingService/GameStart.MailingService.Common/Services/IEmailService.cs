using GameStart.Shared.MessageBus.Models.EmailModels;

namespace GameStart.MailingService.Common.Services
{
    public interface IEmailService
    {
        Task<bool> SendMailAsync(EmailTemplate data, CancellationToken cancellationToken = default);
    }
}
