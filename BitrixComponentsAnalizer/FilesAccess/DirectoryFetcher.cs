using System.IO;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class DirectoryFetcher : IDirectoryFetcher
    {
        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }
    }
}
