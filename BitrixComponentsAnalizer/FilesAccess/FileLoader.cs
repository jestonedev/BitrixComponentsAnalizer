using System;
using System.Collections.Generic;
using System.IO;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using File = BitrixComponentsAnalizer.FilesAccess.ValueObjects.File;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FileLoader: IFileLoader
    {
        private readonly IDirectoryFetcher _directoryFetcher;

        public FileLoader(IDirectoryFetcher directoryFetcher)
        {
            if (directoryFetcher == null)
            {
                throw new ArgumentNullException("directoryFetcher");
            }
            _directoryFetcher = directoryFetcher;
        }

        public IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard)
        {
            var resultFiles = new List<File>();
            foreach (var path in searchPath)
            {
                var findedFiles = _directoryFetcher.GetFiles(path.AbsolutePath,
                    searchFileWildcard, SearchOption.AllDirectories);
                foreach (var findedFile in findedFiles)
                {
                    var ignore = false;
                    foreach (var ignoreRelativePath in path.IgnoreRelativePaths)
                    {
                        var ignoreAbsolutePath = Path.Combine(path.AbsolutePath, ignoreRelativePath);
                        if (!findedFile.StartsWith(ignoreAbsolutePath)) continue;
                        ignore = true;
                        break;
                    }
                    if (!ignore)
                    {
                        resultFiles.Add(new File
                        {
                            FileName = findedFile
                        });
                    }
                }
            }
            return resultFiles;
        }
    }
}
