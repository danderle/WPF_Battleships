using System.Windows.Controls;

namespace Battleships;

/// <summary>
/// Interaction logic for MainMenuPage.xaml
/// </summary>
public partial class BluePage : AnimationPage
{
    public BluePage()
    {
        InitializeComponent();
        DataContext = Inject.Service<MainMenuViewModel>();
    }
}
