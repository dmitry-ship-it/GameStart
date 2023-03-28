using Microsoft.AspNetCore.Http;

namespace GameStart.FilesService.Common.Services
{
    public interface IFileService
    {
        Task SaveFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken = default);

        Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
