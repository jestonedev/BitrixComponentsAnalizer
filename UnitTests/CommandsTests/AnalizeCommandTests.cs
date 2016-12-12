using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CommandsTests
{
    [TestClass]
    public class AnalizeCommandTests
    {
        [TestMethod]
        public void TestExecute()
        {
            var e = new AutoResetEvent(false);
            var onErrorCalled = false;
            var onCompleteCalled = false;
            IEnumerable<BitrixComponent> componentsCollection = null;
            var command = new AnalizeCommand(
                () =>
                {
                    return Task<IEnumerable<BitrixComponent>>.Factory.StartNew(() =>
                        new List<BitrixComponent>
                        {
                            new BitrixComponent
                            {
                                Name = "list",
                                Category = "bitrix:news"
                            }
                        });
                },
                components =>
                {
                    componentsCollection = components;
                    onCompleteCalled = true;
                    e.Set();
                },
                exception =>
                {
                    onErrorCalled = true;
                    e.Set();
                });
            command.Execute(null);
            Assert.IsTrue(e.WaitOne());
            Assert.AreEqual(true, onCompleteCalled);
            Assert.AreEqual(false, onErrorCalled);
            Assert.AreEqual(1, componentsCollection.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestExecuteAnalizeActionNull()
        {
            var command = new AnalizeCommand(
                null,
                components =>
                {
                },
                exception =>
                {
                });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestExecuteOnCompleteNull()
        {
            var command = new AnalizeCommand(
                () => Task<IEnumerable<BitrixComponent>>.Factory.StartNew(() =>new List<BitrixComponent>()),
                null,
                exception =>
                {
                });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestExecuteOnErrorNull()
        {
            var command = new AnalizeCommand(
                () => Task<IEnumerable<BitrixComponent>>.Factory.StartNew(() => new List<BitrixComponent>()),
                components =>
                {
                },
                null);
        }

        [TestMethod]
        public void TestExecuteTaskUnhandledError()
        {
            var e = new AutoResetEvent(false);
            var onErrorCalled = false;
            var onCompleteCalled = false;
            Exception handledException = null;
            var command = new AnalizeCommand(
                () => Task<IEnumerable<BitrixComponent>>.Factory.StartNew(() =>
                {
                    throw new ApplicationException();
                }),
                components =>
                {
                    onCompleteCalled = true;
                    e.Set();
                },
                exception =>
                {
                    onErrorCalled = true;
                    handledException = exception;
                    e.Set();
                });
            command.Execute(null);
            Assert.IsTrue(e.WaitOne());
            Assert.AreEqual(false, onCompleteCalled);
            Assert.AreEqual(true, onErrorCalled);
            Assert.AreEqual(typeof(ApplicationException), (handledException.InnerException ?? handledException).GetType());
        }

        [TestMethod]
        public void TestCanExecuteChanged()
        {
            var e = new AutoResetEvent(false);
            var canExecuteStates = new List<bool>();
            var canExecuteChangesCount = 0;
            var command = new AnalizeCommand(
                () => Task<IEnumerable<BitrixComponent>>.Factory.StartNew(() => new List<BitrixComponent>()),
                components =>
                {
                },
                exception =>
                {
                });
            command.CanExecuteChanged += (sender, args) =>
            {
                canExecuteStates.Add(command.CanExecute(null));
                canExecuteChangesCount++;
                if (canExecuteChangesCount >= 2)
                    e.Set();
            };
            canExecuteStates.Add(command.CanExecute(null));
            command.Execute(null);
            Assert.IsTrue(e.WaitOne());
            Assert.AreEqual(3, canExecuteStates.Count);
            Assert.AreEqual(true, canExecuteStates[0]);
            Assert.AreEqual(false, canExecuteStates[1]);
            Assert.AreEqual(true, canExecuteStates[2]);
        }
    }
}
