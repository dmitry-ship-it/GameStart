namespace GameStart.FilesService.Common.Services
{
    public interface IFileStorage
    {
        bool Exists(string fullPath);

        Task<byte[]> ReadToEndAsync(string fullPath,  CancellationToken cancellationToken = default);

        Stream Open(string fullPath, FileMode mode);
    }
}
