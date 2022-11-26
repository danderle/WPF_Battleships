using System.Windows.Controls;

namespace Battleships;

/// <summary>
/// Interaction logic for BattlePage.xaml
/// </summary>
public partial class BattlePage : Page
{
    public BattlePage()
    {
        InitializeComponent();
        DataContext = Inject.Service<BattleViewModel>();
    }
}
