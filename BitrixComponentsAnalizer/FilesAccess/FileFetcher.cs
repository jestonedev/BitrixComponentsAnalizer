using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FileFetcher : IFileFetcher
    {
        public void WriteTextFile(string fileName, string text)
        {
            System.IO.File.WriteAllText(fileName, text);
        }

        public string ReadTextFile(string fileName)
        {
            return System.IO.File.ReadAllText(fileName);
        }
    }
}
