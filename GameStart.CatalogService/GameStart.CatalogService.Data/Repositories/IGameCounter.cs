namespace GameStart.CatalogService.Data.Repositories
{
    public interface IGameCounter
    {
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}
