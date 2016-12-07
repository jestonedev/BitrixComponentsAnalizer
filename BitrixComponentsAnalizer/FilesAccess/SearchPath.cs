using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class SearchPath : ISearchPath
    {
        public string AbsolutePath { get; set; }
        public IEnumerable<string> IgnoreRelativePaths { get; set; }
    }
}
