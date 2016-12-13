using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests.BitrixInfrastructure
{
    [TestClass]
    public class BitrixBitrixHelperTests
    {
        [TestMethod]
        public void TestBitrixHelper()
        {
            var fileManager = new StubFileSystem();
            fileManager.WriteTextFile("file1.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news\", \"list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;\r\n" +
                               "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news.list\", \"any\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            fileManager.WriteTextFile("file2.php", "<?\r\n" +
                                "require($_SERVER[\"DOCUMENT_ROOT\"].\"/bitrix/header.php\");");
            fileManager.WriteTextFile("file3.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            var bitrixFilesFilter = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), fileManager);
            var files = bitrixFilesFilter.BindComponents(new[]
            {
                new File
                {
                    FileName = "file1.php"
                },
                new File
                {
                    FileName = "file3.php"
                }
            }, (progress, total) => { }).ToList();
            var components = BitrixComponentsAnalizer.BitrixInfrastructure.
                BitrixHelper.InvertFilesAndComponentsCollection(files).ToList();
            Assert.AreEqual(3, components.Count);
            Assert.AreEqual(1, components[0].Files.Count);
            Assert.AreEqual(1, components[1].Files.Count);
            Assert.AreEqual(1, components[2].Files.Count);
            Assert.AreEqual(components[1].Files[0], components[0].Files[0]);
        }

        [TestMethod]
        public void TestBitrixHelperEmptyComponentsList()
        {
            var invertedList = BitrixComponentsAnalizer.BitrixInfrastructure.
                BitrixHelper.InvertFilesAndComponentsCollection(new List<BitrixFile>());
            Assert.AreEqual(0, invertedList.Count());
        }
    }
}
