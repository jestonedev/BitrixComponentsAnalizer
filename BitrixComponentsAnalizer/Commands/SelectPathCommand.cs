using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace BitrixComponentsAnalizer.Commands
{
    internal sealed class SelectPathCommand: ICommand
    {
        public void Execute(object parameter)
        {
            var setPath = parameter as Action<string>;
            if (setPath == null)
            {
                throw new ArgumentException();
            }
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            var path = dialog.SelectedPath;
            setPath(path);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
