using System.Windows;

namespace Battleships
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = Inject.Service<MainWindowViewModel>();
            vm.SetWindow(this);
            DataContext = vm;
        }
    }
}
