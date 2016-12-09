using System;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests
{
    [TestClass]
    public class FilesAccessUnitTests
    {
        [TestMethod]
        public void TestFileLoader()
        {
            var directoryFetcher = new FakeDirectoryFetcher();
            var fileLoader = new FileLoader(directoryFetcher);
            var files = fileLoader.GetFiles(new[]
            {
                new SearchPath
                {
                    AbsolutePath = "/",
                    IgnoreRelativePaths = new[] {"ignore", "/anydir/ignore"}
                }
            }, "*.php", (progressIntoPath, totalIntoPath, path) => {}).ToList();
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
