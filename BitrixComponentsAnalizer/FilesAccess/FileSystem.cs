using System.IO;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FileSystem : IFileSystem
    {
        public void WriteTextFile(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }

        public string ReadTextFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }
    }
}
