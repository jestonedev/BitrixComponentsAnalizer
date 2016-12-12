using System;
using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    internal class StubFileLoader: IFileLoader
    {
        public IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard, 
            Action<double, double, bool, string> progressCallback)
        {
            var files = new[] { "/1.php", "/ignore/text.php", "/ignore/any/text.php", 
                "/anydir/ignore/fuck.php", "/anydir/any/fuck.php" }.Select(fileName =>
                new BitrixComponentsAnalizer.FilesAccess.ValueObjects.File
                {
                    FileName = fileName
                });
            progressCallback(100, 100, false, "Anypath");
            return files;
        }
    }
}
