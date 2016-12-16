using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IBitrixComponentValidator
    {
        bool ComponentExists(BitrixComponent component, string[] templateAbsolutePaths, bool skipDefaultComponentName);
    }
}
