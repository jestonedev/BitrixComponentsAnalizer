using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IBitrixFilesStorage
    {
        IEnumerable<BitrixComponent> LoadComponents();
        void SaveComponents(IEnumerable<BitrixComponent> components);
    }
}
