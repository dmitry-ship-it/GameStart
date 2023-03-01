using GameStart.Shared.MessageBus;
using GameStart.Shared.MessageBus.Models.EmailModels;
using MassTransit;

namespace GameStart.IdentityService.Common.Publishers
{
    public class EmailVerificationRequestPublisher : IMessagePublisher<EmailTemplate>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public EmailVerificationRequestPublisher(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task PublishMessageAsync(EmailTemplate message, CancellationToken cancellationToken = default)
        {
            await publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
