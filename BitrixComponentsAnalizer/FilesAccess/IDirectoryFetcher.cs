using System.IO;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public interface IDirectoryFetcher
    {
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
    }
}
