using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Battleships
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private MainWindow _window;

        public MainWindowViewModel()
        {
        }

        public void SetWindow(MainWindow mainWindow)
        {
            if (mainWindow == null)
            {
                _window = mainWindow;
            }
        }
    }
}
