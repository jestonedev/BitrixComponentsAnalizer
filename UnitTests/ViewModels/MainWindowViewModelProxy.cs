using System;
using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.Common.Interfaces;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.ViewModels;

namespace UnitTests.ViewModels
{
    internal class MainWindowViewModelProxy: MainWindowViewModel
    {
        public MainWindowViewModelProxy(
            IFileSystem fileSytem,
            IFileLoader fileLoader,
            IBitrixFilesStorage bitrixFilesStorage,
            IBitrixFilesComponentsBinder bitrixFilesComponentsBinder,
            IBitrixComponentValidator bitrixComponentValidator,
            ILogger logger, ISettings settings):
            base(fileSytem, fileLoader, bitrixFilesStorage, bitrixFilesComponentsBinder, bitrixComponentValidator, logger, settings)
        {
        }

        public new void AnalizeCommandComplete(IEnumerable<BitrixComponent> components)
        {
            base.AnalizeCommandComplete(components);
        }

        public new void AnalizeCommandError(Exception exception)
        {
            base.AnalizeCommandError(exception);
        }

        public new void SelectPathCommandComplete(string path)
        {
            base.SelectPathCommandComplete(path);
        }
    }
}
