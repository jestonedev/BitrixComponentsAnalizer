using System;
using System.Collections.Generic;
using System.IO;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using File = BitrixComponentsAnalizer.FilesAccess.ValueObjects.File;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FileLoader: IFileLoader
    {
        private readonly IFileSystem _fileSystem;

        public FileLoader(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            _fileSystem = fileSystem;
        }

        public IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard, 
            Action<double, double, bool, string> progressCallback)
        {
            var resultFiles = new List<File>();
            foreach (var path in searchPath)
            {
                progressCallback(0, int.MaxValue, true, path.AbsolutePath);
                var findedFiles = _fileSystem.GetFiles(path.AbsolutePath,
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
                    progressCallback(i, totalCount, false, path.AbsolutePath);
                }
            }
            return resultFiles;
        }
    }
}
