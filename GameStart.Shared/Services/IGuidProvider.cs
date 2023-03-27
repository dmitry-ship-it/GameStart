namespace GameStart.Shared.Services
{
    public interface IGuidProvider
    {
        Guid New { get; }

        Guid Empty { get; }
    }
}
