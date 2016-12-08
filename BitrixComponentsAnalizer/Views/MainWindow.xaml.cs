using System;
using System.Collections.Generic;
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
using BitrixComponentsAnalizer.Presenters;

namespace BitrixComponentsAnalizer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowPresenter _presenter;

        public MainWindow()
        {
            _presenter = new MainWindowPresenter(this);
            InitializeComponent();
        }

        public MainWindow(MainWindowPresenter presenter)
        {
            if (presenter == null)
            {
                throw new ArgumentNullException("presenter");
            }
            _presenter = presenter;
            InitializeComponent();
        }
    }
}
