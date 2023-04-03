namespace GameStart.Shared.Services
{
    public interface IGuidProvider
    {
        Guid NewGuid { get; }

        Guid Empty { get; }
    }
}
