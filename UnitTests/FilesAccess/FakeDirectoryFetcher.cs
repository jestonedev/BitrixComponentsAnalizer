using System.IO;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    internal class FakeDirectoryFetcher: IDirectoryFetcher
    {
        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return new[] { "/1.php", "/ignore/text.php", "/ignore/any/text.php", 
                "/anydir/ignore/fuck.php", "/anydir/any/fuck.php" };
        }
    }
}
