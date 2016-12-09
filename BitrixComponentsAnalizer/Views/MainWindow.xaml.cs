using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.Commands;
using BitrixComponentsAnalizer.FilesAccess;
using BitrixComponentsAnalizer.Presenters;
using BitrixComponentsAnalizer.Views.Interfaces;

namespace BitrixComponentsAnalizer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        public string SelectedPath
        {
            get { return (string) GetValue(_selectedPathValuePropery); }
            set { SetValue(_selectedPathValuePropery, value); }
        }

        private readonly DependencyProperty _selectedPathValuePropery;


        public IEnumerable<BitrixComponent> Components
        {
            get { return (IEnumerable<BitrixComponent>)GetValue(_componentsCollection); }
            set { SetValue(_componentsCollection, value); }
        }

        private readonly DependencyProperty _componentsCollection;

        private readonly MainWindowPresenter _presenter;

        public MainWindow(): this(null)
        {
        }

        public MainWindow(MainWindowPresenter presenter)
        {
            _selectedPathValuePropery = DependencyProperty.Register("SelectedPath", typeof(string),
               typeof(MainWindow));
            _componentsCollection = DependencyProperty.Register("Components",
                typeof(IEnumerable<BitrixComponent>), typeof(MainWindow));
            if (presenter == null)
            {
                _presenter = new MainWindowPresenter(this,
                    new BitrixFilesStorage("last_analize.json", new FileFetcher()),
                    new FileLoader(new DirectoryFetcher()),
                    new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), new FileFetcher()));
            }
            else
            {
                _presenter = presenter;
            }
            InitializeComponent();
        }

        private ICommand _analizeCommand;
        public ICommand AnalizeCommand
        {
            get
            {
                return _analizeCommand ?? (_analizeCommand = new AnalizeCommand());
            }
        }

        private ICommand _selectPathCommand;

        public ICommand SelectPathCommand
        {
            get
            {
                return _selectPathCommand ?? (_selectPathCommand = new SelectPathCommand());
            }
        }

        public Action<string> AfterSelectPathAction
        {
            get { return AfteSelectPath; }
        }

        private void AfteSelectPath(string path)
        {
            SelectedPath = path;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var task = _presenter.LoadStoredComponentsAsync();
            _presenter.LoadStoredComponentsAsync().GetAwaiter().OnCompleted(() =>
            {
                Components = task.Result;
            });
        }
    }
}
