namespace GameStart.Shared.Services
{
    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid => Guid.NewGuid();

        public Guid Empty => Guid.Empty;
    }
}
