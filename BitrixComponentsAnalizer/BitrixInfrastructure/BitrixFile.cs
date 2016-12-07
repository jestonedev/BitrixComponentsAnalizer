using System.Collections.Generic;
using BitrixComponentsAnalizer.FilesAccess;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFile: IFile
    {
        public string FileName { get; set; }
        public IReadOnlyCollection<BitrixComponent> Components { get; set; }
    }
}
