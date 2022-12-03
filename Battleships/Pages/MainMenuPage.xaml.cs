using System.Windows.Controls;

namespace Battleships;

/// <summary>
/// Interaction logic for MainMenuPage.xaml
/// </summary>
public partial class MainMenuPage : AnimationPage
{
    public MainMenuPage()
    {
        InitializeComponent();
        DataContext = Inject.Service<MainMenuViewModel>();
    }
}
