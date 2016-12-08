using System.IO;

namespace BitrixComponentsAnalizer.FilesAccess.Interfaces
{
    public interface IDirectoryFetcher
    {
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
    }
}
