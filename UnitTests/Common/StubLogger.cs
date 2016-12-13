using BitrixComponentsAnalizer.Common.Interfaces;

namespace UnitTests.Common
{
    internal class StubLogger: ILogger
    {
        public delegate void LoggingHandler(string message);

        public event LoggingHandler Logging;

        public void Log(string message)
        {
            var handler = Logging;
            if (handler != null)
            {
                handler(message);
            }
        }
    }
}
