using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitrixComponentsAnalizer.Views;

namespace BitrixComponentsAnalizer.Presenters
{
    public class MainWindowPresenter
    {
        private readonly MainWindow _mainWindow;

        public MainWindowPresenter(MainWindow mainWindow)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException("mainWindow");
            }
            _mainWindow = mainWindow;
        }
    }
}
