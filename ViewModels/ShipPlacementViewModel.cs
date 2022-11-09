using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships
{
    public partial class ShipPlacementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.MainMenuPage;

        public ShipPlacementViewModel()
        {

        }
    }
}
