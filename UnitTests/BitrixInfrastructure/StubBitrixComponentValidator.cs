using System;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace UnitTests.BitrixInfrastructure
{
    internal class StubBitrixComponentValidator: IBitrixComponentValidator
    {
        public bool ComponentExists(BitrixComponent component, string[] templateAbsolutePaths)
        {
            return true;
        }
    }
}
