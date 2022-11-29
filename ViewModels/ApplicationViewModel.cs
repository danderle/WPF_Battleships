using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        #region Properties
        public bool Istart { get; set; }
        public string MyName { get; set; }
        public string OpponentName { get; set; }
        public ClientToServer Server { get; private set; } = new ClientToServer();
        public List<ShipViewModel> MySetShips { get; set; }

        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.MainMenuPage;

        #endregion

        #region Constructor

        public ApplicationViewModel()
        {
            Server.WhoStartsAction = WhoStarts;
        }

        #endregion

        #region Action methods

        private void WhoStarts(string nameOfWhoStarts)
        {
            Istart = MyName == nameOfWhoStarts;
        }

        #endregion
    }
}
