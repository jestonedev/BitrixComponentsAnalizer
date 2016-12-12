using System.IO;

namespace BitrixComponentsAnalizer.FilesAccess.Interfaces
{
    public interface IFileSystem
    {
        void WriteTextFile(string fileName, string text);
        string ReadTextFile(string fileName);
        bool FileExists(string fileName);
        bool DirectoryExists(string path);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
    }
}
