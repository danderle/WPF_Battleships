using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships
{
    public partial class ShipPlacementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ShipGridViewModel shipGrid = new ShipGridViewModel();

        public ShipPlacementViewModel()
        {

        }
    }
}
