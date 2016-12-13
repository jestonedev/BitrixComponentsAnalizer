using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests.BitrixInfrastructure
{
    [TestClass]
    public class BitrixFilesStorageTests
    {
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
            var bitrixFilesStorage = new BitrixFilesStorage(null, new StubFileSystem());
        }

        [TestMethod]
        public void TestBitrixFilesStorageRelative()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("1.json", new StubFileSystem());
            var bitrixComponents = BitrixHelper.GetBitrixComponentsSample();
            bitrixFilesStorage.SaveComponents(bitrixComponents);
            var loadedBitrixFiles = bitrixFilesStorage.LoadComponents().ToList();
            Assert.AreEqual(bitrixComponents.Count, loadedBitrixFiles.Count);
            for (var i = 0; i < bitrixComponents.Count; i++)
            {
                Assert.AreEqual(bitrixComponents[i].Name, loadedBitrixFiles[i].Name);
                Assert.AreEqual(bitrixComponents[i].Category, loadedBitrixFiles[i].Category);
                Assert.AreEqual(bitrixComponents[i].Files != null ? 
                    bitrixComponents[i].Files.Count : 0,
                    loadedBitrixFiles[i].Files != null ? loadedBitrixFiles[i].Files.Count : 0);
            }
        }

        [TestMethod]
        public void TestBitrixFilesStorageAbsolute()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("/1.json", new StubFileSystem());
            var bitrixComponents = BitrixHelper.GetBitrixComponentsSample();
            bitrixFilesStorage.SaveComponents(bitrixComponents);
            var loadedBitrixFiles = bitrixFilesStorage.LoadComponents().ToList();
            Assert.AreEqual(bitrixComponents.Count, loadedBitrixFiles.Count);
            for (var i = 0; i < bitrixComponents.Count; i++)
            {
                Assert.AreEqual(bitrixComponents[i].Name, loadedBitrixFiles[i].Name);
                Assert.AreEqual(bitrixComponents[i].Category, loadedBitrixFiles[i].Category);
                Assert.AreEqual(bitrixComponents[i].Files != null ?
                    bitrixComponents[i].Files.Count : 0,
                    loadedBitrixFiles[i].Files != null ? loadedBitrixFiles[i].Files.Count : 0);
            }
        }

        [TestMethod]
        public void TestBitrixFilesStorageComponentsNotFound()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("/1.json", new StubFileSystem());
            var storedComponents = bitrixFilesStorage.LoadComponents();
            Assert.AreEqual(0, storedComponents.Count());
        }
    }
}
