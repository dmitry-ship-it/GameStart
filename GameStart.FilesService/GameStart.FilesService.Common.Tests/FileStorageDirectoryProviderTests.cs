using GameStart.FilesService.Common.Services;
using System.Reflection;

namespace GameStart.FilesService.Common.Tests
{
    public class FileStorageDirectoryProviderTests
    {
        public FileStorageDirectoryProviderTests()
        {
            Environment.SetEnvironmentVariable("HOME", "/home");
            Environment.SetEnvironmentVariable("HOMEDRIVE", "C:");
            Environment.SetEnvironmentVariable("HOMEPATH", @"\Users\testuser");
        }

        [Fact]
        public void Constructor_WhenRunningOnUnixOrMacOSX_ShouldSetDirectoryToCorrectValue()
        {
            // Arrange
            var operatingSystem = new OperatingSystem(
                PlatformID.Unix, new Version(13, 5, 265));
            typeof(Environment).GetField("s_osVersion", BindingFlags.Static | BindingFlags.NonPublic)!
                .SetValue(null, operatingSystem);

            // Act
            var sut = new FileStorageDirectoryProvider();

            // Assert
            sut.Directory.Should().StartWith("/home").And.EndWith("files");
        }

        [Fact]
        public void Constructor_WhenRunningOnWindows_ShouldSetDirectoryToCorrectValue()
        {
            // Arrange
            var operatingSystem = new OperatingSystem(
                PlatformID.Win32NT, new Version(10, 0));
            typeof(Environment).GetField("s_osVersion", BindingFlags.Static | BindingFlags.NonPublic)!
                .SetValue(null, operatingSystem);

            // Act
            var sut = new FileStorageDirectoryProvider();

            // Assert
            sut.Directory.Should().StartWith(@"C:\Users\testuser").And.EndWith("files");
        }
    }
}
