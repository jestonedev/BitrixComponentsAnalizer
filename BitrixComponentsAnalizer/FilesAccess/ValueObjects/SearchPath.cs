using System.Collections.Generic;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.FilesAccess.ValueObjects
{
    public class SearchPath : ISearchPath
    {
        public string AbsolutePath { get; set; }
        public IEnumerable<string> IgnoreRelativePaths { get; set; }
    }
}
