using System;
using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.FilesAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests
{
    [TestClass]
    public class BitrixInfrastrucutreTests
    {
        [TestMethod]
        public void TestComponentExtractor()
        {
            var extractor = new BitrixComponentExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION->IncludeComponent(\"bitrix:news\", \"list\", array(").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
        }

        [TestMethod]
        public void TestSpacesComponentExtractor()
        {
            var extractor = new BitrixComponentExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent ( \"bitrix:news\", \"list\", array(\r\n"+
                "\"IBLOCK_TYPE\" => \"feedback\",").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
        }

        [TestMethod]
        public void TestTwoLineComponentExtractor()
        {
            var extractor = new BitrixComponentExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent\r\n "+
                "( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
        }

        [TestMethod]
        public void TestTwoComponentsComponentExtractor()
        {
            var extractor = new BitrixComponentExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",\r\n"+
                "<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news.list\", \"any\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",").ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
            Assert.AreEqual("any", components[1].Name);
            Assert.AreEqual("bitrix:news.list", components[1].Category);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesFilterNullExtractor()
        {
            var bitrixFilesFilter = new BitrixFilesFilter(null, new FakeFileManager());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesFilterNullFileManager()
        {
            var bitrixFilesFilter = new BitrixFilesFilter(new BitrixComponentExtractor(), null);
        }

        [TestMethod]
        public void TestBitrixFilesFilter()
        {
            var fileManager = new FakeFileManager();
            var bitrixFilesFilter = new BitrixFilesFilter(new BitrixComponentExtractor(), fileManager);
            fileManager.WriteTextFile("file1.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news\", \"list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",\r\n" +
                               "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news.list\", \"any\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",");
            fileManager.WriteTextFile("file2.php", "<?\r\n" +
                                "require($_SERVER[\"DOCUMENT_ROOT\"].\"/bitrix/header.php\");");
            fileManager.WriteTextFile("file3.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",");
            var files = bitrixFilesFilter.FilterFiles(new[]
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
            }).ToList();
            Assert.AreEqual(2, files.Count);
            Assert.AreEqual(2, files[0].Components.Count);
            Assert.AreEqual("list", files[0].Components.ToList()[0].Name);
            Assert.AreEqual("bitrix:news", files[0].Components.ToList()[0].Category);
            Assert.AreEqual("any", files[0].Components.ToList()[1].Name);
            Assert.AreEqual("bitrix:news.list", files[0].Components.ToList()[1].Category);
            Assert.AreEqual(1, files[1].Components.Count);
            Assert.AreEqual("list.list", files[1].Components.ToList()[0].Name);
            Assert.AreEqual("bitrix:date.picker", files[1].Components.ToList()[0].Category);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesStorageNullFileManager()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("1.json", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesStorageNullFileName()
        {
            var bitrixFilesStorage = new BitrixFilesStorage(null, new FakeFileManager());
        }

        [TestMethod]
        public void TestBitrixFilesStorage()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("1.json", new FakeFileManager());
            var bitrixFiles = new List<BitrixFile>
            {
                new BitrixFile
                {
                    FileName = "1.php",
                    Components = new[]
                    {
                        new BitrixComponent
                        {
                            Name = "list",
                            Category = "bitrix:news"
                        }
                    }
                },
                new BitrixFile
                {
                    FileName = "2.php",
                    Components = new[]
                    {
                        new BitrixComponent
                        {
                            Name = "any",
                            Category = "bitrix:news.list"
                        }
                    }
                }
            };
            bitrixFilesStorage.SaveFiles(bitrixFiles);
            var loadedBitrixFiles = bitrixFilesStorage.LoadFiles().ToList();
            if (loadedBitrixFiles.Count != bitrixFiles.Count)
            {
                Assert.Fail("Not equal count of files into persistent storage");
            }
            for (var i = 0; i < bitrixFiles.Count; i++)
            {
                Assert.AreEqual(bitrixFiles[0].FileName, loadedBitrixFiles[0].FileName);
                Assert.AreEqual(bitrixFiles[0].Components.Count, loadedBitrixFiles[0].Components.Count);
                Assert.AreEqual(bitrixFiles[0].Components.ToList()[0].Name, 
                    loadedBitrixFiles[0].Components.ToList()[0].Name);
                Assert.AreEqual(bitrixFiles[0].Components.ToList()[0].Category,
                    loadedBitrixFiles[0].Components.ToList()[0].Category);
                Assert.AreEqual(bitrixFiles[1].FileName, loadedBitrixFiles[1].FileName);
                Assert.AreEqual(bitrixFiles[1].Components.Count, loadedBitrixFiles[1].Components.Count);
                Assert.AreEqual(bitrixFiles[1].Components.ToList()[0].Name,
                    loadedBitrixFiles[1].Components.ToList()[0].Name);
                Assert.AreEqual(bitrixFiles[1].Components.ToList()[0].Category,
                    loadedBitrixFiles[1].Components.ToList()[0].Category);
            }
        }
    }
}
