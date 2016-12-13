using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace UnitTests.BitrixInfrastructure
{
    internal class StubBitrixFilesStorage : IBitrixFilesStorage
    {
        private IEnumerable<BitrixComponent> _components;

        public IEnumerable<BitrixComponent> LoadComponents()
        {
            return _components;
        }

        public void SaveComponents(IEnumerable<BitrixComponent> components)
        {
            _components = components;
        }
    }
}
