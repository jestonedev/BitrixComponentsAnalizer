namespace BitrixComponentsAnalizer.FilesAccess.Interfaces
{
    public interface IFileFetcher
    {
        void WriteTextFile(string fileName, string text);
        string ReadTextFile(string fileName);
    }
}
