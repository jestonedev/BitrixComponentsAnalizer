using System;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilesAccess
{
    [TestClass]
    public class FilesAccessUnitTests
    {
        [TestMethod]
        public void TestFileLoader()
        {
            var fileSystem = new StubFileSystem();
            foreach (var fileName in new[] { "/1.php", "/ignore/text.php", "/ignore/any/text.php", 
                "/anydir/ignore/fuck.php", "/anydir/any/fuck.php" })
            {
                fileSystem.WriteTextFile(fileName, "");
            }
            var fileLoader = new FileLoader(fileSystem);
            var files = fileLoader.GetFiles(new[]
            {
                new SearchPath
                {
                    AbsolutePath = "/",
                    IgnoreRelativePaths = new[] {"ignore", "anydir/ignore"}
                }
            }, "*.php", (progressIntoPath, totalIntoPath, isDeterministic, path) => {}).ToList();
            Assert.AreEqual(2, files.Count);
            Assert.AreEqual("/1.php", files[0].FileName);
            Assert.AreEqual("/anydir/any/fuck.php", files[1].FileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFileLoaderException()
        {
            var fileLoader = new FileLoader(null);
        }
    }
}
