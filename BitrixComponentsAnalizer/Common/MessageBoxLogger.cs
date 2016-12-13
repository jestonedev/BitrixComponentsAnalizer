using System.Windows;
using BitrixComponentsAnalizer.Common.Interfaces;

namespace BitrixComponentsAnalizer.Common
{
    internal class MessageBoxLogger: ILogger
    {
        public void Log(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
