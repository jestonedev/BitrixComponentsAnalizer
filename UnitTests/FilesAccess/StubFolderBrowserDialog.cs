using System.Windows.Forms;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    internal class StubFolderBrowserDialog: IFolderBrowserDialog
    {
        public string SelectedPath
        {
            get { return "/any/path"; }
        }

        public DialogResult ShowDialog()
        {
            return DialogResult.OK;
        }
    }
}
