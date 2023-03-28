namespace GameStart.FilesService.Common.Services
{
    public class FileStorageDirectoryProvider : IFileStorageDirectoryProvider
    {
        public FileStorageDirectoryProvider()
        {
            var homePath = (Environment.OSVersion.Platform is PlatformID.Unix or PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")!
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")!;

            Directory = Path.Combine(homePath, "files");
        }

        public string Directory { get; }
    }
}
