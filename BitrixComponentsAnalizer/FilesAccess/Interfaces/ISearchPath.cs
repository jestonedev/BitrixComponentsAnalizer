using System.Collections.Generic;

namespace BitrixComponentsAnalizer.FilesAccess.Interfaces
{
    public interface ISearchPath
    {
        string AbsolutePath { get; set; }
        IEnumerable<string> IgnoreRelativePaths { get; set; }
    }
}
