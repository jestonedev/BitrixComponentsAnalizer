using System;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests.BitrixInfrastructure
{
    [TestClass]
    public class BitrixFilesComponentsBinderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesFilterNullExtractor()
        {
            var bitrixFilesComponentsBinder = new BitrixFilesComponentsBinder(null, new StubFileSystem());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesFilterNullFileManager()
        {
            var bitrixFilesComponentsBinder = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), null);
        }

        [TestMethod]
        public void TestBitrixFilesFilter()
        {
            var fileManager = new StubFileSystem();
            var bitrixFilesComponentsBinder = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), fileManager);
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
            var files = bitrixFilesComponentsBinder.BindComponents(new[]
            {
                new File
                {
                    FileName = "file1.php"
                },
                new File
                {
                    FileName = "file2.php"
                },
                new File
                {
                    FileName = "file3.php"
                }
            }, (progress, total) => { }).ToList();
            Assert.AreEqual(3, files.Count);
            Assert.AreEqual(2, files[0].Components.Count);
            Assert.AreEqual("list", files[0].Components.ToList()[0].Name);
            Assert.AreEqual("bitrix:news", files[0].Components.ToList()[0].Category);
            Assert.AreEqual("any", files[0].Components.ToList()[1].Name);
            Assert.AreEqual("bitrix:news.list", files[0].Components.ToList()[1].Category);
            Assert.AreEqual(0, files[1].Components.Count);
            Assert.AreEqual(1, files[2].Components.Count);
            Assert.AreEqual("list.list", files[2].Components.ToList()[0].Name);
            Assert.AreEqual("bitrix:date.picker", files[2].Components.ToList()[0].Category);
        }

        [TestMethod]
        public void TestComponentsBindingBitrixFilesFilter()
        {
            var fileManager = new StubFileSystem();
            var bitrixFilesComponentsBinder = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), fileManager);
            fileManager.WriteTextFile("file1.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news\", \"list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;\r\n" +
                               "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            fileManager.WriteTextFile("file3.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            var files = bitrixFilesComponentsBinder.BindComponents(new[]
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
            Assert.AreEqual(2, files[0].Components[1].Files.Count);
            Assert.AreSame(files[0].Components[1], files[1].Components[0]);
        }
    }
}
