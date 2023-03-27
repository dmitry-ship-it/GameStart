namespace GameStart.Shared.Services
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
    }
}
