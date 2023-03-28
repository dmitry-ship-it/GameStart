using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStart.FilesService.Common.Services
{
    public interface IFileStorageDirectoryProvider
    {
        string Directory { get; }
    }
}
