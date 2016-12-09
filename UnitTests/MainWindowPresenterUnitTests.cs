using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BitrixInfrastructure;
using UnitTests.FilesAccess;
using UnitTests.Views;

namespace UnitTests
{
    [TestClass]
    public class MainWindowPresenterUnitTests
    {
        [TestMethod]
        public void TestLoadFilesAsync()
        {
            var fileStorage = new FakeBitrixFilesStorage();
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
            var presenter = new MainWindowPresenter(new FakeMainWindow(), 
                fileStorage, new FakeFileLoader(), 
                new FakeBitrixFilesComponentsBinder());           
            var components = presenter.LoadStoredComponentsAsync().Result.ToList();
            Assert.AreEqual(2, components.Count);           
        }

        [TestMethod]
        public void TestAnalizeAsync()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var presenter = new MainWindowPresenter(new FakeMainWindow(),
                new FakeBitrixFilesStorage(), new FakeFileLoader(),
                new FakeBitrixFilesComponentsBinder());
            presenter.FilesListLoadProgressChangedEvent += e =>
            {
                Assert.IsTrue(e.TotalIntoPath >= e.ProgressIntoPath);
                Assert.AreEqual("Anypath", e.Path);
            };
            presenter.ComponentsBindProgressChangedEvent += e =>
            {
                Assert.IsTrue(e.Total >= e.Progress);
            };
            var components = presenter.AnalizeAsync().Result.ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(4, components[0].Files.Count);
            Assert.AreEqual(3, components[1].Files.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullMainWindow()
        {
            var presenter = new MainWindowPresenter(null,
                new FakeBitrixFilesStorage(), new FakeFileLoader(),
                new FakeBitrixFilesComponentsBinder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullBitrixFilesStorage()
        {
            var presenter = new MainWindowPresenter(new FakeMainWindow(), 
                null, new FakeFileLoader(),
                new FakeBitrixFilesComponentsBinder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullFileLoader()
        {
            var presenter = new MainWindowPresenter(new FakeMainWindow(),
                new FakeBitrixFilesStorage(), null,
                new FakeBitrixFilesComponentsBinder());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPresenterNullBitrixFilesComponentsBinder()
        {
            var presenter = new MainWindowPresenter(new FakeMainWindow(),
                new FakeBitrixFilesStorage(), new FakeFileLoader(), 
                null);
        }
    }
}
