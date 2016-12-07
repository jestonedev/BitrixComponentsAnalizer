namespace BitrixComponentsAnalizer.FilesAccess
{
    public interface IFileManager
    {
        void WriteTextFile(string fileName, string text);
        string LoadTextFile(string fileName);
    }
}
