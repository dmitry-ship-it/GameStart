﻿namespace GameStart.FilesService.Common.Services
{
    public interface IFileStorageDirectoryProvider
    {
        string Directory { get; }

        public void CreateRootDirectoryIfNotExists();
    }
}
