using System.Windows.Forms;

namespace BitrixComponentsAnalizer.FilesAccess.Interfaces
{
    public interface IFolderBrowserDialog
    {
        string SelectedPath { get; }
        DialogResult ShowDialog();
    }
}
