using System;
using System.Collections.Generic;

namespace BitrixComponentsAnalizer.FilesAccess.Interfaces
{
    public interface IFileLoader
    {
        IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard, 
            Action<double, double, bool, string> progressCallback);
    }
}
