using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.Presenters;
using BitrixComponentsAnalizer.Views.Interfaces;

namespace UnitTests.Presenters
{
    internal class MainWindowPresenterSyncWrapper: MainWindowPresenter
    {
        public MainWindowPresenterSyncWrapper(IMainWindow mainWindow, IBitrixFilesStorage bitrixFilesStorage, 
            IFileLoader fileLoader, 
            IBitrixFilesComponentsBinder bitrixFilesComponentsBinder) : 
            base(mainWindow, bitrixFilesStorage, fileLoader, bitrixFilesComponentsBinder)
        {
        }

        public new IEnumerable<BitrixComponent> LoadStoredComponentsAsync()
        {
            return LoadStoredComponents();
        }

        public new IEnumerable<BitrixComponent> AnalizeAsync()
        {
            return Analize();
        }
    }
}
