using System.Collections.Generic;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IComponentsExtractor
    {
        IEnumerable<IComponent> GetComponentsFromCode(string code);
    }
}
