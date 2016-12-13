using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.Commands;
using BitrixComponentsAnalizer.Common;
using BitrixComponentsAnalizer.Common.Interfaces;
using BitrixComponentsAnalizer.FilesAccess;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;
using Newtonsoft.Json;

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
            get
            {
                return _components ?? (_components = new ObservableCollection<BitrixComponent>());
            }
            set
            {
                _components = value;
                _components.CollectionChanged += (s, e) =>
                {
                    RaisePropertyChanged("Components");
                    RaisePropertyChanged("FilteredComponents");
                };
                RaisePropertyChanged("Components");
                RaisePropertyChanged("FilteredComponents");
            }
        }

        private ObservableCollection<BitrixComponent> _components;

        public ObservableCollection<BitrixComponent> FilteredComponents
        {
            get
            {
                if (string.IsNullOrEmpty(Filter))
                {
                    return Components;
                }
                var filter = Filter.ToUpperInvariant();
                return new ObservableCollection<BitrixComponent>(_components.
                    Where(c => (c.Name != null && c.Name.ToUpperInvariant().Contains(filter)) ||
                               (c.Category != null && c.Category.ToUpperInvariant().Contains(filter)) ||
                               (c.Files != null && c.Files.Any(f =>
                                   f.FileName != null && f.FileName.ToUpperInvariant().Contains(filter)))));
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                RaisePropertyChanged("Filter");
                RaisePropertyChanged("FilteredComponents");
            }
        }

        private string _filter;

        public ObservableCollection<BitrixTemplate> Templates
        {
            get
            {
                return _templates ?? (_templates = new ObservableCollection<BitrixTemplate>());
            }
            set
            {
                _templates = value;
                _templates.CollectionChanged += (s, e) =>
                {
                    RaisePropertyChanged("Templates");
                };
                RaisePropertyChanged("Templates");
            }
        }

        private ObservableCollection<BitrixTemplate> _templates;

        private readonly IFileSystem _fileSystem;
        private readonly IFileLoader _fileLoader;
        private readonly IBitrixFilesStorage _bitrixFilesStorage;
        private readonly IBitrixFilesComponentsBinder _bitrixFilesComponentsBinder;
        private readonly IBitrixComponentValidator _bitrixComponentValidator;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        private readonly SynchronizationContext _context = SynchronizationContext.Current;

        public MainWindowViewModel()
            : this(new FileSystem(),
                new FileLoader(new FileSystem()),
                new BitrixFilesStorage("last_analize.json", new FileSystem()),
                new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), new FileSystem()),
                new BitrixComponentValidator(new FileSystem()), 
                new MessageBoxLogger(), 
                new ApplicationSettings())
        {
        }

        public MainWindowViewModel(
            IFileSystem fileSytem,
            IFileLoader fileLoader,
            IBitrixFilesStorage bitrixFilesStorage,
            IBitrixFilesComponentsBinder bitrixFilesComponentsBinder,
            IBitrixComponentValidator bitrixComponentValidator,
            ILogger logger, ISettings settings)
        {
            if (fileSytem == null)
            {
                throw new ArgumentNullException("fileSytem");
            }
            _fileSystem = fileSytem;

            if (fileLoader == null)
            {
                throw new ArgumentNullException("fileLoader");
            }
            _fileLoader = fileLoader;

            if (bitrixFilesStorage == null)
            {
                throw new ArgumentNullException("bitrixFilesStorage");
            }
            _bitrixFilesStorage = bitrixFilesStorage;

            if (bitrixFilesComponentsBinder == null)
            {
                throw new ArgumentNullException("bitrixFilesComponentsBinder");
            }
            _bitrixFilesComponentsBinder = bitrixFilesComponentsBinder;

            if (bitrixComponentValidator == null)
            {
                throw new ArgumentNullException("bitrixComponentValidator");
            }
            _bitrixComponentValidator = bitrixComponentValidator;

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            _settings = settings;

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _logger = logger;
        }

        public async Task<IEnumerable<BitrixComponent>> LoadStoredComponentsAsync()
        {
            
            return await Task<IEnumerable<BitrixComponent>>.Factory.StartNew(LoadStoredComponentsTask);
        }

        private IEnumerable<BitrixComponent> LoadStoredComponentsTask()
        {
            return _bitrixFilesStorage.LoadComponents();
        }

        private CancellationTokenSource _analizeCancelationTokenSource;

        public void CancelAnalize()
        {
            if (_analizeCancelationTokenSource != null)
            {
                _analizeCancelationTokenSource.Cancel();
            }
        }

        public async Task<IEnumerable<BitrixComponent>> AnalizeAsync()
        {
            _analizeCancelationTokenSource = new CancellationTokenSource();
            return await Task<IEnumerable<BitrixComponent>>.Factory.StartNew(AnalizeTask, 
                _analizeCancelationTokenSource.Token);
        }

        private IEnumerable<BitrixComponent> AnalizeTask()
        {
            return GetComponents(GetFiles());
        }

        private IEnumerable<BitrixComponent> GetComponents(IEnumerable<IFile> files)
        {
            var components = BitrixHelper.InvertFilesAndComponentsCollection(
                _bitrixFilesComponentsBinder.BindComponents(files,
                    (progress, total) =>
                    {
                        _context.Post(state =>
                        {
                            AnalizeProgressCurrent = ((dynamic) state).progress;
                            AnalizeProgressTotal = ((dynamic) state).total;
                            AnalizeProgressIsIndeterminate = false;
                            AnalizeProgressStatusMessage = "Анализ наличия компонентов";
                        }, new {progress, total});
                    })).ToList();
            var templateAbsolutePaths = Templates.Select(t => 
                Path.Combine(SelectedPath, "bitrix\\templates\\", t.Name, "components")).ToArray();
            foreach (var component in components)
            {
                component.IsExists = _bitrixComponentValidator.ComponentExists(
                    component, 
                    templateAbsolutePaths);
            }
            return components;
        }

        private IEnumerable<IFile> GetFiles()
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
            return files;
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
                    AnalizeCommandComplete, AnalizeCommandError));
            }
        }

        protected virtual void AnalizeCommandComplete(IEnumerable<BitrixComponent> components)
        {
            Components = new ObservableCollection<BitrixComponent>(components);
            AnalizeProgressStatusMessage = "Анализ завершен";
            AnalizeProgressCurrent = 0;
            AnalizeProgressTotal = int.MaxValue;
        }

        protected virtual void AnalizeCommandError(Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            _logger.Log(exception.Message);
            if (!(exception is DirectoryNotFoundException))
            {
                throw exception;
            }
        }

        private ICommand _selectPathCommand;

        public ICommand SelectPathCommand
        {
            get
            {
                return _selectPathCommand ?? (_selectPathCommand = new SelectPathCommand(
                    new FolderBrowserDialog(), SelectPathCommandComplete));
            }
        }

        protected virtual void SelectPathCommandComplete(string path)
        {
            SelectedPath = path;
        }

        public void SaveState()
        {
            _bitrixFilesStorage.SaveComponents(Components);
            _settings.SetValue("TemplatesJson", JsonConvert.SerializeObject(Templates));
            _settings.SetValue("SelectedPath", SelectedPath);
        }

        public async void LoadState()
        {
            var templates = JsonConvert.DeserializeObject<ObservableCollection<BitrixTemplate>>
                (_settings.GetValue("TemplatesJson").ToString());
            if (templates != null)
                Templates = templates;
            SelectedPath = (string)_settings.GetValue("SelectedPath");
            Components = new ObservableCollection<BitrixComponent>(await LoadStoredComponentsAsync());
        }
    }
}
