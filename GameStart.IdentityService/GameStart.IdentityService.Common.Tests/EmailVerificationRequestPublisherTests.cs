using GameStart.IdentityService.Common.Publishers;
using GameStart.Shared.MessageBus.Models.EmailModels;
using MassTransit;

namespace GameStart.IdentityService.Common.Tests
{
    public class EmailVerificationRequestPublisherTests
    {
        [Fact]
        public async Task PublishMessageAsync_ShouldPublishMessageTemplate()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var email = new EmailTemplate
            {
                To = "example@mail.com",
                Subject = "Test subject",
                Body = "Lorem ipsum"
            };

            var publishEndpointMock = Substitute.For<IPublishEndpoint>();
            var sut = new EmailVerificationRequestPublisher(publishEndpointMock);

            // Act
            await sut.PublishMessageAsync(email, cancellationToken);

            // Assert
            await publishEndpointMock.Received().Publish(email, cancellationToken);
        }
    }
}
