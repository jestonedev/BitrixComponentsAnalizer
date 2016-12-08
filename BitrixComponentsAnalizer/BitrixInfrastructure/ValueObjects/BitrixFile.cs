using System.Collections.ObjectModel;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects
{
    public class BitrixFile: IFile
    {
        public string FileName { get; set; }
        public ReadOnlyCollection<BitrixComponent> Components { get; set; }
    }
}
