using System.Collections.Generic;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public interface IBitrixFilesStorage
    {
        IEnumerable<BitrixFile> LoadFiles();
        void SaveFiles(IEnumerable<BitrixFile> files);
    }
}
