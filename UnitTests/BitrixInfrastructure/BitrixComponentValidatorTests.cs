using System;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests.BitrixInfrastructure
{
    [TestClass]
    public class BitrixComponentValidatorTests
    {
        [TestMethod]
        public void TestComponentExists()
        {
            var validator = new BitrixComponentsAnalizer.BitrixInfrastructure.
                BitrixComponentValidator(new StubFileSystem());
            Assert.AreEqual(true, validator.ComponentExists(new BitrixComponent
            {
                Name = "Any",
                Category = "Name"
            }, new[] { ".default" }));
            Assert.AreEqual(false, validator.ComponentExists(new BitrixComponent
            {
                Name = "Notexists",
                Category = "Name",
            }, new[] { ".default" }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFileSystem()
        {
            var validator = new BitrixComponentsAnalizer.BitrixInfrastructure.BitrixComponentValidator(null);
        }
    }
}
