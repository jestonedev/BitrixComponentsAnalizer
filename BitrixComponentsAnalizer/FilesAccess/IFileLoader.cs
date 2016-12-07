using System.Collections.Generic;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public interface IFileLoader
    {
        IEnumerable<IFile> GetFiles(IEnumerable<ISearchPath> searchPath, string searchFileWildcard);
    }
}
