using System.Windows.Controls;

namespace Battleships;

/// <summary>
/// Interaction logic for ShipPlacementPage.xaml
/// </summary>
public partial class ShipPlacementPage : AnimationPage
{
    public ShipPlacementPage()
    {
        InitializeComponent();
        DataContext = Inject.Service<ShipPlacementViewModel>();
    }
}
