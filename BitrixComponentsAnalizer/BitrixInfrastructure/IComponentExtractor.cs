using System.Collections.Generic;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public interface IComponentExtractor
    {
        IEnumerable<IComponent> GetComponentsFromCode(string code);
    }
}
