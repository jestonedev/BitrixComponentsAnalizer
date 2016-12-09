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

        public IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard, 
            Action<double, double, string> progressCallback)
        {
            var resultFiles = new List<File>();
            foreach (var path in searchPath)
            {
                var findedFiles = _directoryFetcher.GetFiles(path.AbsolutePath,
                    searchFileWildcard, SearchOption.AllDirectories);
                var totalCount = findedFiles.Length - 1;
                for (var i = 0; i < findedFiles.Length; i++)
                {
                    var findedFile = findedFiles[i];
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
                    progressCallback(i, totalCount, path.AbsolutePath);
                }
            }
            return resultFiles;
        }
    }
}
