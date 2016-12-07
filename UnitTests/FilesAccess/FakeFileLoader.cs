using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitrixComponentsAnalizer.FilesAccess;

namespace UnitTests.FilesAccess
{
    class FakeFileLoader: IFileLoader
    {
        public IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard)
        {
            var directoryFetcher = new FakeDirectoryFetcher();
            return directoryFetcher.GetFiles("", "", SearchOption.AllDirectories).Select(fileName =>
                new BitrixComponentsAnalizer.FilesAccess.File
                {
                    FileName = fileName
                });
        }
    }
}
