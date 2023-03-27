namespace GameStart.Shared.Services
{
    public class GuidProvider : IGuidProvider
    {
        public Guid New => Guid.NewGuid();

        public Guid Empty => Guid.Empty;
    }
}
