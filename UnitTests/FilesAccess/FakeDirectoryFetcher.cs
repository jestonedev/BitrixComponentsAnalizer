using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitrixComponentsAnalizer.FilesAccess;

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
