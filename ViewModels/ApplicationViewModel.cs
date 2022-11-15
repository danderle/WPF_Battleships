using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.ShipPlacementPage;

        public ApplicationViewModel()
        {

        }
    }
}
