using GameStart.MailingService.Common.Services;
using GameStart.Shared.MessageBus.Models.EmailModels;
using Hangfire;
using MassTransit;

namespace GameStart.MailingService.Common.Consumers
{
    public class EmailVerificationConsumer : IConsumer<EmailTemplate>
    {
        private readonly IEmailService emailService;
        private readonly IBackgroundJobClient backgroundJob;

        public EmailVerificationConsumer(
            IBackgroundJobClient backgroundJob,
            IEmailService emailService)
        {
            this.backgroundJob = backgroundJob;
            this.emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailTemplate> context)
        {
            backgroundJob.Enqueue(() =>
                emailService.SendMailAsync(context.Message, context.CancellationToken));

            await Task.CompletedTask;
        }
    }
}
