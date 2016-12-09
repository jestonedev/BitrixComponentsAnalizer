namespace BitrixComponentsAnalizer.EventArgs
{
    public class FilesListLoadProgressChangedEventArg: System.EventArgs
    {
        public double ProgressIntoPath { get; private set; }
        public double TotalIntoPath { get; private set; }
        public string Path { get; private set; }

        public FilesListLoadProgressChangedEventArg(double progressIntoPath, double totalIntoPath, string path)
        {
            ProgressIntoPath = progressIntoPath;
            TotalIntoPath = totalIntoPath;
            Path = path;
        }
    }
}
