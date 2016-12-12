using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.Commands;
using BitrixComponentsAnalizer.FilesAccess;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;

namespace BitrixComponentsAnalizer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _analizeProgressIsIndeterminate;
        public bool AnalizeProgressIsIndeterminate
        {
            get { return _analizeProgressIsIndeterminate; }
            set
            {
                _analizeProgressIsIndeterminate = value;
                RaisePropertyChanged("AnalizeProgressIsIndeterminate");
            }
        }

        private double _analizeProgreesCurrent;      
        public double AnalizeProgressCurrent
        {
            get { return _analizeProgreesCurrent; }
            set
            {
                _analizeProgreesCurrent = value;
                RaisePropertyChanged("AnalizeProgressCurrent");
            }
        }

        private double _analizeProgreesTotal = int.MaxValue;
        public double AnalizeProgressTotal
        {
            get { return _analizeProgreesTotal; }
            set
            {
                _analizeProgreesTotal = value;
                RaisePropertyChanged("AnalizeProgressTotal");
            }
        }

        private string _analizeProgressStatusMessage = "Прогресс";
        public string AnalizeProgressStatusMessage
        {
            get { return _analizeProgressStatusMessage; }
            set
            {
                _analizeProgressStatusMessage = value;
                RaisePropertyChanged("AnalizeProgressStatusMessage");
            }
        }

        public string SelectedPath
        {
            get
            {
                return _selectedPath;
            }
            set
            {
                _selectedPath = value;
                RaisePropertyChanged("SelectedPath");
            }
        }

        private string _selectedPath;


        public ObservableCollection<BitrixComponent> Components
        {
            get { return _components; }
            set
            {
                _components = value;
                RaisePropertyChanged("Components");
            }
        }

        private ObservableCollection<BitrixComponent> _components;

        private readonly IFileSystem _fileSystem;
        private readonly IBitrixFilesStorage _bitrixFilesStorage;
        private readonly IFileLoader _fileLoader;
        private readonly IBitrixFilesComponentsBinder _bitrixFilesComponentsBinder;

        private readonly SynchronizationContext _context = SynchronizationContext.Current;

        public MainWindowViewModel()
            : this(new FileSystem(), 
                new BitrixFilesStorage("last_analize.json", new FileSystem()),
                new FileLoader(new FileSystem()),
                new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), new FileSystem()))
        {
        }

        public MainWindowViewModel(
            IFileSystem fileSytem,
            IBitrixFilesStorage bitrixFilesStorage,
            IFileLoader fileLoader,
            IBitrixFilesComponentsBinder bitrixFilesComponentsBinder)
        {
            if (fileSytem == null)
            {
                throw new ArgumentNullException("fileSytem");
            }
            _fileSystem = fileSytem;

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

        public void LoadStoredComponents()
        {
            var task = LoadStoredComponentsAsync();
            task.GetAwaiter().OnCompleted(() =>
            {
                Components = new ObservableCollection<BitrixComponent>(task.Result);
            });
        }

        public async Task<IEnumerable<BitrixComponent>> LoadStoredComponentsAsync()
        {
            return await Task<IEnumerable<BitrixComponent>>.Factory.StartNew(LoadStoredComponentsTask);
        }

        private IEnumerable<BitrixComponent> LoadStoredComponentsTask()
        {
            return BitrixHelper.InvertFilesAndComponentsCollection(_bitrixFilesStorage.LoadFiles());
        }

        public void Analize()
        {
            var task = AnalizeAsync();
            task.GetAwaiter().OnCompleted(() =>
            {
                Components = new ObservableCollection<BitrixComponent>(task.Result);
            });
        }

        public async Task<IEnumerable<BitrixComponent>> AnalizeAsync()
        {
            return await Task<IEnumerable<BitrixComponent>>.Factory.StartNew(AnalizeTask);
        }

        private IEnumerable<BitrixComponent> AnalizeTask()
        {
            if (!_fileSystem.DirectoryExists(SelectedPath))
            {
                throw new DirectoryNotFoundException();
            }
            var searchPaths = new List<ISearchPath>
            {
                new SearchPath
                {
                    AbsolutePath = SelectedPath,
                    IgnoreRelativePaths = new[] {"bitrix\\"}
                }
            };
            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - 1;
            var files = _fileLoader.GetFiles(searchPaths, "*.php", (progress, total, isIndeterminate, path) =>
            {
                var nextUnixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                if (unixTimestamp == nextUnixTimestamp) return;
                unixTimestamp = nextUnixTimestamp;
                _context.Post(state =>
                {
                    AnalizeProgressCurrent = ((dynamic) state).progress;
                    AnalizeProgressTotal = ((dynamic) state).total;
                    AnalizeProgressIsIndeterminate = ((dynamic) state).isIndeterminate;
                    AnalizeProgressStatusMessage = string.Format("Получение перечня файлов директории {0}",
                        ((dynamic) state).path);
                }, new {progress, total, path, isIndeterminate});
            });
            var components = BitrixHelper.InvertFilesAndComponentsCollection(
                _bitrixFilesComponentsBinder.BindComponents(files,
                    (progress, total) =>
                    {
                        _context.Post(state =>
                        {
                            AnalizeProgressCurrent = ((dynamic)state).progress;
                            AnalizeProgressTotal = ((dynamic)state).total;
                            AnalizeProgressIsIndeterminate = false;
                            AnalizeProgressStatusMessage = "Анализ наличия компонентов";
                        }, new {progress, total});
                    }));
            return components;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ICommand _analizeCommand;
        public ICommand AnalizeCommand
        {
            get
            {
                return _analizeCommand ?? (_analizeCommand = new AnalizeCommand(AnalizeAsync,
                    components =>
                    {
                        Components = new ObservableCollection<BitrixComponent>(components);
                    },
                    exception =>
                    {
                        while (exception.InnerException != null)
                        {
                            exception = exception.InnerException;
                        }
                        MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (!(exception is DirectoryNotFoundException))
                        {
                            throw exception;
                        }
                    }));
            }
        }

        private ICommand _selectPathCommand;

        public ICommand SelectPathCommand
        {
            get
            {
                return _selectPathCommand ?? (_selectPathCommand = new SelectPathCommand(
                    new FolderBrowserDialog(), 
                    path =>
                {
                    SelectedPath = path;
                }));
            }
        }
    }
}
