using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace Battleships
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region Fields

        private const double PADDING = 10;
        private const double CORNER_RADIUS = 7;
        private const double RESIZE_BORDER = 15;
        private const double CAPTION_HEIGHT = 30;

        private MainWindow _window;

        #endregion

        #region Properties

        [ObservableProperty]
        private Thickness padding = new Thickness(PADDING);

        [ObservableProperty]
        private CornerRadius cornerRadius = new CornerRadius(CORNER_RADIUS);

        [ObservableProperty]
        private Thickness resizeBorder = new Thickness(RESIZE_BORDER);

        [ObservableProperty]
        private double captionHeight = CAPTION_HEIGHT;

        #endregion

        #region Methods

        public void SetWindow(MainWindow mainWindow)
        {
            if (_window == null)
            {
                _window = mainWindow;
                _window.SizeChanged += _window_SizeChanged;
            }
        }

        private void _window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_window.WindowState == WindowState.Maximized)
            {
                CornerRadius = new CornerRadius(0);
                ResizeBorder = new Thickness(0);
                Padding = new Thickness(6);
            }
            else
            {
                CornerRadius = new CornerRadius(CORNER_RADIUS);
                ResizeBorder = new Thickness(RESIZE_BORDER);
                Padding = new Thickness(PADDING);
            }
        }

        private Point GetMousePosition()
        {
            var position = Mouse.GetPosition(_window);
            return new Point(position.X + _window.Left, position.Y + _window.Top);
        }

        #endregion

        #region Commands

        [RelayCommand]
        public void Minimize() => _window.WindowState = WindowState.Minimized;

        [RelayCommand]
        public void Maximize() => _window.WindowState = WindowState.Maximized;

        [RelayCommand]
        public void Close() => _window.Close();

        [RelayCommand]
        public void SystemMenu() => SystemCommands.ShowSystemMenu(_window, GetMousePosition());

        #endregion


    }
}
