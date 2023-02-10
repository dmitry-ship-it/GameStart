namespace GameStart.Shared.MessageBus
{
    public interface IMessagePublisher<in T>
    {
        Task PublishMessageAsync(T message, CancellationToken cancellationToken = default);
    }
}
