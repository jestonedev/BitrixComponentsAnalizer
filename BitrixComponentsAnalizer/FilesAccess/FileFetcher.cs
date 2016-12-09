using System.IO;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FileFetcher : IFileFetcher
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
    }
}
