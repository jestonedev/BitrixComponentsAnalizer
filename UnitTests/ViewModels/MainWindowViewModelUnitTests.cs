using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BitrixInfrastructure;
using UnitTests.FilesAccess;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class MainWindowViewModelUnitTests
    {
        [TestMethod]
        public void TestLoadFilesAsync()
        {
            var fileStorage = new StubBitrixFilesStorage();
            var bitrixFiles = new List<BitrixFile>
            {
                new BitrixFile
                {
                    FileName = "1.php",
                    Components = new List<BitrixComponent>
                    {
                        new BitrixComponent
                        {
                            Name = "list",
                            Category = "bitrix:news"
                        }
                    }.AsReadOnly()
                },
                new BitrixFile
                {
                    FileName = "2.php",
                    Components = new List<BitrixComponent>
                    {
                        new BitrixComponent
                        {
                            Name = "any",
                            Category = "bitrix:news.list"
                        }
                    }.AsReadOnly()
                }
            };
            fileStorage.SaveFiles(bitrixFiles);
            var presenter = new MainWindowViewModel(
                new StubFileSystem(), 
                fileStorage, new StubFileLoader(), 
                new StubBitrixFilesComponentsBinder());           
            var components = presenter.LoadStoredComponentsAsync().Result.ToList();
            Assert.AreEqual(2, components.Count);           
        }

        [TestMethod]
        public void TestAnalizeAsync()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var presenter = new MainWindowViewModel(
                new StubFileSystem(), 
                new StubBitrixFilesStorage(), new StubFileLoader(),
                new StubBitrixFilesComponentsBinder());
            var components = presenter.AnalizeAsync().Result.ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(4, components[0].Files.Count);
            Assert.AreEqual(3, components[1].Files.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullFileSystem()
        {
            var presenter = new MainWindowViewModel(
                null,
                new StubBitrixFilesStorage(), new StubFileLoader(),
                new StubBitrixFilesComponentsBinder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullBitrixFilesStorage()
        {
            var presenter = new MainWindowViewModel(
                new StubFileSystem(), 
                null, new StubFileLoader(),
                new StubBitrixFilesComponentsBinder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullFileLoader()
        {
            var presenter = new MainWindowViewModel(
                new StubFileSystem(), 
                new StubBitrixFilesStorage(), null,
                new StubBitrixFilesComponentsBinder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullBitrixFilesComponentsBinder()
        {
            var presenter = new MainWindowViewModel(
                new StubFileSystem(), 
                new StubBitrixFilesStorage(), new StubFileLoader(), 
                null);
        }
    }
}
