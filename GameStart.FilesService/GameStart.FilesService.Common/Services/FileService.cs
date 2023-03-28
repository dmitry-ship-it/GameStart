using Microsoft.AspNetCore.Http;

namespace GameStart.FilesService.Common.Services
{
    public class FileService : IFileService
    {
        private readonly IFileStorageDirectoryProvider directoryProvider;

        public FileService(IFileStorageDirectoryProvider directoryProvider)
        {
            this.directoryProvider = directoryProvider;
        }

        public async Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(directoryProvider.Directory, fileName);

            return File.Exists(path)
                ? await File.ReadAllBytesAsync(path, cancellationToken)
                : null;
        }

        public async Task SaveFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken = default)
        {
            Directory.CreateDirectory(directoryProvider.Directory);
            var filePath = Path.Combine(directoryProvider.Directory, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
        }
    }
}
