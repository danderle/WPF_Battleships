using System.Windows;

namespace Battleships
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Inject.Setup();

            base.OnStartup(e);
        }
    }
}
