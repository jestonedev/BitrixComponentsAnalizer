using System;
using System.Threading;
using BitrixComponentsAnalizer.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests.CommandsTests
{
    [TestClass]
    public class SelectPathCommandTests
    {
        [TestMethod]
        public void TestExecute()
        {
            var e = new AutoResetEvent(false);
            string selectedPath = null;
            var command = new SelectPathCommand(
                new StubFolderBrowserDialog(), 
                path =>
                {
                    selectedPath = path;
                    e.Set();
                });
            command.Execute(null);
            Assert.IsTrue(e.WaitOne());
            Assert.AreEqual("/any/path", selectedPath);
        }

        [TestMethod]
        public void TestCancelDialogExecute()
        {
            var e = new AutoResetEvent(false);
            string selectedPath = null;
            var command = new SelectPathCommand(
                new StubCancelFolderBrowserDialog(),
                path =>
                {
                    selectedPath = path;
                    e.Set();
                });
            command.Execute(null);
            Assert.IsFalse(e.WaitOne(1000));
            Assert.AreEqual(null, selectedPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFolderBrowserDialogNull()
        {
            var command = new SelectPathCommand(null, path => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPathSelectedNull()
        {
            var command = new SelectPathCommand(new StubFolderBrowserDialog(), null);
        }

        [TestMethod]
        public void TestCanExecute()
        {
            var command = new SelectPathCommand(new StubFolderBrowserDialog(), path => { });
            Assert.AreEqual(true, command.CanExecute(null));
        }
    }
}
