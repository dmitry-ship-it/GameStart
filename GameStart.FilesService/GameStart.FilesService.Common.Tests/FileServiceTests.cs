using GameStart.FilesService.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace GameStart.FilesService.Common.Tests
{
    public class FileServiceTests
    {
        private readonly IFileStorageDirectoryProvider directoryProviderMock;
        private readonly IFileStorage fileStorageMock;
        private readonly IFileService sut;

        public FileServiceTests()
        {
            directoryProviderMock = Substitute.For<IFileStorageDirectoryProvider>();
            fileStorageMock = Substitute.For<IFileStorage>();
            sut = new FileService(directoryProviderMock, fileStorageMock);
        }

        [Fact]
        public async Task GetFileAsync_WhenFileExists_ShouldReturnFileContent()
        {
            // Arrange
            const string fileName = "test.txt";
            var cancellationToken = CancellationToken.None;
            var path = Path.Combine(directoryProviderMock.Directory, fileName);
            var fileContent = new byte[] { 1, 2, 3 };
            fileStorageMock.Exists(path).Returns(true);
            fileStorageMock.ReadToEndAsync(path, cancellationToken).Returns(fileContent);

            // Act
            var result = await sut.GetFileAsync(fileName, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(fileContent);
        }

        [Fact]
        public async Task GetFileAsync_WhenFileDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            const string fileName = "test.txt";
            var cancellationToken = CancellationToken.None;
            var path = Path.Combine(directoryProviderMock.Directory, fileName);
            fileStorageMock.Exists(path).Returns(false);

            // Act
            var result = await sut.GetFileAsync(fileName, cancellationToken);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SaveFileAsync_ShouldSaveFileToCorrectLocation()
        {
            // Arrange
            const string fileName = "test.txt";
            var cancellationToken = CancellationToken.None;
            var file = Substitute.For<IFormFile>();
            directoryProviderMock.Directory.Returns(@"C:\Test");

            // Act
            await sut.SaveFileAsync(file, fileName, cancellationToken);

            // Assert
            var expectedPath = Path.Combine(directoryProviderMock.Directory, fileName);
            fileStorageMock.Received(1).Open(expectedPath, FileMode.Create);
        }

        [Fact]
        public async Task SaveFileAsync_ShouldCopyFileToStream()
        {
            // Arrange
            const string fileName = "test.txt";
            var cancellationToken = CancellationToken.None;
            var file = Substitute.For<IFormFile>();
            directoryProviderMock.Directory.Returns(@"C:\Test");

            // Act
            await sut.SaveFileAsync(file, fileName, cancellationToken);

            // Assert
            await file.Received(1).CopyToAsync(Arg.Any<Stream>(), cancellationToken);
        }
    }
}
