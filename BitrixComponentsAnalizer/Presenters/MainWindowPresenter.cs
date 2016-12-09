using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.EventArgs;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.Views.Interfaces;

namespace BitrixComponentsAnalizer.Presenters
{
    public class MainWindowPresenter
    {
        private readonly IMainWindow _mainWindow;
        private readonly IBitrixFilesStorage _bitrixFilesStorage;
        private readonly IFileLoader _fileLoader;
        private readonly IBitrixFilesComponentsBinder _bitrixFilesComponentsBinder;

        public delegate void FilesListLoadProgressChangedEventHandler(FilesListLoadProgressChangedEventArg e);
        public event FilesListLoadProgressChangedEventHandler FilesListLoadProgressChangedEvent;

        public delegate void ComponentsBindProgressChangedEventHandler(ComponentsBindProgressChangedEventArg e);
        public event ComponentsBindProgressChangedEventHandler ComponentsBindProgressChangedEvent;

        private readonly SynchronizationContext _context = SynchronizationContext.Current;

        public MainWindowPresenter(IMainWindow mainWindow, 
            IBitrixFilesStorage bitrixFilesStorage,
            IFileLoader fileLoader,
            IBitrixFilesComponentsBinder bitrixFilesComponentsBinder)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException("mainWindow");
            }
            _mainWindow = mainWindow;

            if (bitrixFilesStorage == null)
            {
                throw new ArgumentNullException("bitrixFilesStorage");
            }
            _bitrixFilesStorage = bitrixFilesStorage;

            if (fileLoader == null)
            {
                throw new ArgumentNullException("fileLoader");
            }
            _fileLoader = fileLoader;

            if (bitrixFilesComponentsBinder == null)
            {
                throw new ArgumentNullException("bitrixFilesComponentsBinder");
            }
            _bitrixFilesComponentsBinder = bitrixFilesComponentsBinder;
        }

        public async Task<IEnumerable<BitrixComponent>> LoadStoredComponentsAsync()
        {
            return await Task<IEnumerable<BitrixComponent>>.Factory.StartNew(LoadStoredComponents);
        }

        protected IEnumerable<BitrixComponent> LoadStoredComponents()
        {
            return BitrixHelper.InvertFilesAndComponentsCollection(_bitrixFilesStorage.LoadFiles());
        }

        public async Task<IEnumerable<BitrixComponent>> AnalizeAsync()
        {
            return await Task<IEnumerable<BitrixComponent>>.Factory.StartNew(Analize);
        }

        protected IEnumerable<BitrixComponent> Analize()
        {
            var files = _fileLoader.GetFiles(null/*TODO:get search path from mainform*/, "*.php", (progressIntoPath, totalIntoPath, path) =>
            {
                _context.Post(state =>
                {
                    if (FilesListLoadProgressChangedEvent != null)
                        FilesListLoadProgressChangedEvent(new FilesListLoadProgressChangedEventArg(
                            progressIntoPath, totalIntoPath, path));
                }, new {progress = progressIntoPath, total = totalIntoPath});
            });
            var components = BitrixHelper.InvertFilesAndComponentsCollection(
                _bitrixFilesComponentsBinder.BindComponents(files,
                    (progress, total) =>
                    {
                        _context.Post(state =>
                        {
                            if (ComponentsBindProgressChangedEvent != null)
                                ComponentsBindProgressChangedEvent(
                                    new ComponentsBindProgressChangedEventArg(progress, total));
                        }, new {progress, total});
                    }));
            return components;
        }
    }
}
