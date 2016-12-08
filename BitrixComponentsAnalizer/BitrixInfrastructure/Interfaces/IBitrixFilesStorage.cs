using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IBitrixFilesStorage
    {
        IEnumerable<BitrixFile> LoadFiles();
        void SaveFiles(IEnumerable<BitrixFile> files);
    }
}
