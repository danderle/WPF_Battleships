using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        #region Properties

        public string MyName { get; set; }
        public string OpponentName { get; set; }
        public ClientToServer Server { get; private set; } = new ClientToServer();
        public SignalRServer SignalR { get; private set; } = new SignalRServer();
        public List<ShipViewModel> MySetShips { get; set; }

        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.MainMenuPage;

        #endregion
    }
}
