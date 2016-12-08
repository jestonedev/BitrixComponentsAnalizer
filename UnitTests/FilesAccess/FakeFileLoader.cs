using System.Collections.Generic;
using System.IO;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    internal class FakeFileLoader: IFileLoader
    {
        public IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard)
        {
            var directoryFetcher = new FakeDirectoryFetcher();
            return directoryFetcher.GetFiles("", "", SearchOption.AllDirectories).Select(fileName =>
                new BitrixComponentsAnalizer.FilesAccess.ValueObjects.File
                {
                    FileName = fileName
                });
        }
    }
}
