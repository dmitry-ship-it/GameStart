namespace GameStart.FilesService.Common.Services
{
    public class FileStorage : IFileStorage
    {
        public bool Exists(string fullPath) => File.Exists(fullPath);

        public Stream Open(string fullPath, FileMode mode)
        {
            return new FileStream(fullPath, mode);
        }

        public async Task<byte[]> ReadToEndAsync(string fullPath, CancellationToken cancellationToken = default)
        {
            return await File.ReadAllBytesAsync(fullPath, cancellationToken);
        }
    }
}
