using System;
using System.Windows.Forms;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.FilesAccess
{
    public class FolderBrowserDialog: IFolderBrowserDialog, IDisposable
    {
        private readonly System.Windows.Forms.FolderBrowserDialog _dialog;
        private bool _disposed;
        public FolderBrowserDialog()
        {
            _dialog = new System.Windows.Forms.FolderBrowserDialog();
        }


        public string SelectedPath
        {
            get { return _dialog.SelectedPath; }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _dialog.Dispose();   
            }
            _disposed = true;
        }
    }
}
