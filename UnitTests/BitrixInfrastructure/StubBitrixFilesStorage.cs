using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace UnitTests.BitrixInfrastructure
{
    internal class StubBitrixFilesStorage : IBitrixFilesStorage
    {
        private IEnumerable<BitrixFile> _files;

        public IEnumerable<BitrixFile> LoadFiles()
        {
            return _files;
        }

        public void SaveFiles(IEnumerable<BitrixFile> files)
        {
            _files = files;
        }
    }
}
