using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        public string MyName { get; set; }
        public string OpponentName { get; set; }
        public ClientToServer Server { get; private set; } = new ClientToServer();
        public List<ShipViewModel> MySetShips { get; set; }

        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.MainMenuPage;

        public ApplicationViewModel()
        {

        }
    }
}
