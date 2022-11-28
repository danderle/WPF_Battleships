using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        public ClientToServer Server { get; private set; } = new ClientToServer();
        public string OpponentName { get; set; }
        public List<ShipViewModel> MySetShips { get; set; }

        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.BattlePage;

        public ApplicationViewModel()
        {

        }
    }
}
