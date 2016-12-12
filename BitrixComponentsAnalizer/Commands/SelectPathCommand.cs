using System;
using System.Windows.Forms;
using System.Windows.Input;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.Commands
{
    public sealed class SelectPathCommand: ICommand
    {
        private readonly Action<string> _pathSelected;
        private readonly IFolderBrowserDialog _folderBrowserDialog;

        public SelectPathCommand(IFolderBrowserDialog folderBrowserDialog, Action<string> pathSelected)
        {
            if (pathSelected == null)
            {
                throw new ArgumentNullException("pathSelected");
            }
            if (folderBrowserDialog == null)
            {
                throw new ArgumentNullException("folderBrowserDialog");
            }
            _pathSelected = pathSelected;
            _folderBrowserDialog = folderBrowserDialog;
        }

        public void Execute(object parameter)
        {
            if (_folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            var path = _folderBrowserDialog.SelectedPath;
            _pathSelected(path);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
