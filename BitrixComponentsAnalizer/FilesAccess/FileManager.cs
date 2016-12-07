namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FileManager: IFileManager
    {
        public void WriteTextFile(string fileName, string text)
        {
            System.IO.File.WriteAllText(fileName, text);
        }

        public string LoadTextFile(string fileName)
        {
            return System.IO.File.ReadAllText(fileName);
        }
    }
}
