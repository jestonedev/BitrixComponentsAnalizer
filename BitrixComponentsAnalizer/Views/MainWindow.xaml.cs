using System.Windows;
using BitrixComponentsAnalizer.ViewModels;

namespace BitrixComponentsAnalizer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {

            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).LoadStoredComponents();
        }
    }
}
