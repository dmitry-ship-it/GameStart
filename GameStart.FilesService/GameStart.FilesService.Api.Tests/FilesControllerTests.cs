using GameStart.FilesService.Common.Services;
using GameStart.FilesService.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GameStart.FilesService.Api.Tests
{
    public class FilesControllerTests
    {
        private readonly IFileService fileService;
        private readonly FilesController sut;

        public FilesControllerTests()
        {
            fileService = Substitute.For<IFileService>();
            sut = new FilesController(fileService);
        }

        [Fact]
        public async Task GetFileAsync_WhenFileFound_ShouldReturnFile()
        {
            // Arrange
            const string fileName = "ExistingFile.png";
            var cancellationToken = CancellationToken.None;
            var fileBytes = new byte[] { 1, 2, 3 };
            fileService.GetFileAsync(fileName, cancellationToken).Returns(fileBytes);

            // Act
            var result = await sut.GetFileAsync(fileName, cancellationToken);

            // Assert
            result.Should().BeOfType<FileContentResult>();
            var fileResult = (FileContentResult)result;
            fileResult.ContentType.Should().Be(MediaTypeNames.Application.Octet);
            fileResult.FileContents.Should().BeEquivalentTo(fileBytes);
            fileResult.EnableRangeProcessing.Should().BeTrue();
        }

        [Fact]
        public async Task GetFileAsync_WhenFileNotFound_ShouldReturnNotFound()
        {
            // Arrange
            const string fileName = "NonexistentFile.png";
            var cancellationToken = CancellationToken.None;
            fileService.GetFileAsync(fileName, cancellationToken).Returns((byte[])null!);

            // Act
            var result = await sut.GetFileAsync(fileName, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SaveFileAsync_WhenFileSaved_ShouldReturnOkResult()
        {
            // Arrange
            const string fileName = "NewFile.txt";
            var cancellationToken = CancellationToken.None;
            var file = Substitute.For<IFormFile>();
            fileService.SaveFileAsync(file, fileName, cancellationToken).Returns(Task.CompletedTask);

            // Act
            var result = await sut.SaveFileAsync(file, fileName);

            // Assert
            result.Should().BeOfType<OkResult>();
            await fileService.Received().SaveFileAsync(file, fileName, cancellationToken);
        }
    }
}
