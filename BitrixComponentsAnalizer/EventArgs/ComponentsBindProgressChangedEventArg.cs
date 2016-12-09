namespace BitrixComponentsAnalizer.EventArgs
{
    public class ComponentsBindProgressChangedEventArg: System.EventArgs
    {
        public double Progress { get; private set; }
        public double Total { get; private set; }

        public ComponentsBindProgressChangedEventArg(double progress, double total)
        {
            Progress = progress;
            Total = total;
        }
    }
}
