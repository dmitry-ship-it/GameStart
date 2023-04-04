using Microsoft.AspNetCore.Http;

namespace GameStart.FilesService.Common.Services
{
    public class FileService : IFileService
    {
        private readonly IFileStorageDirectoryProvider directoryProvider;
        private readonly IFileStorage fileStorage;

        public FileService(IFileStorageDirectoryProvider directoryProvider, IFileStorage fileStorage)
        {
            this.directoryProvider = directoryProvider;
            this.fileStorage = fileStorage;
        }

        public async Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(directoryProvider.Directory, fileName);

            return fileStorage.Exists(path)
                ? await fileStorage.ReadToEndAsync(path, cancellationToken)
                : null;
        }

        public async Task SaveFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken = default)
        {
            directoryProvider.CreateRootDirectoryIfNotExists();
            var filePath = Path.Combine(directoryProvider.Directory, fileName);

            using var stream = fileStorage.Open(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
        }
    }
}
