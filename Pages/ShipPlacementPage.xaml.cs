using System.Windows.Controls;

namespace Battleships;

/// <summary>
/// Interaction logic for ShipPlacementPage.xaml
/// </summary>
public partial class ShipPlacementPage : Page
{
    public ShipPlacementPage()
    {
        InitializeComponent();
        DataContext = Inject.Service<ShipPlacementViewModel>();
    }
}
