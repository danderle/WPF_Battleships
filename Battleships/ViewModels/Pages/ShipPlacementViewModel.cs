using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Text.Json;

namespace Battleships
{
    public partial class ShipPlacementViewModel : ObservableObject
    {
        #region Fields

        private bool _opponentFinished;

        private string _myName => Inject.Application.Server.Username;

        #endregion

        #region Properties

        [ObservableProperty]
        private bool opponentDisconnected;

        [ObservableProperty]
        private bool waitingMessageVisible;
        
        [ObservableProperty]
        private ShipGridViewModel shipGrid = new ShipGridViewModel();

        public string OpponentName => Inject.Application.OpponentName;

        #endregion

        #region Constructor

        public ShipPlacementViewModel()
        {
            Inject.Application.Server.DisconnectedClientAction = DisconnectedClientAction;
            Inject.Application.Server.FinishedSetupAction = FinishedSetup;
            ShipGrid.Ships = new System.Collections.ObjectModel.ObservableCollection<ShipViewModel>()
            {
                new ShipViewModel(ShipTypes.Carrier, 0),
                new ShipViewModel(ShipTypes.Battleship, 40),
                new ShipViewModel(ShipTypes.Cruiser, 80),
                new ShipViewModel(ShipTypes.Submarine, 120),
                new ShipViewModel(ShipTypes.Destroyer, 160),
            };
        }

        #endregion

        #region Server Actions

        private void DisconnectedClientAction(string disconnectedUsername)
        {
            if (OpponentName == disconnectedUsername)
            {
                OpponentDisconnected = true;
            }
        }

        public void FinishedSetup(string message)
        {
            _opponentFinished = true;
            if (WaitingMessageVisible)
            {
                GoToBattle();
            }
        }

        #endregion

        #region Command method

        [RelayCommand]
        public void Continue()
        {
            var user = new User()
            {
                Name = _myName,
                IsBusy = false,
                Starts = false
            };

            string message = JsonSerializer.Serialize(user);
            Inject.Application.Server.CreateAndSendPacket(OpCodes.Busy, message);
            Inject.Application.CurrentPage = ApplicationPages.MainMenuPage;
        }
        [RelayCommand]
        public void StartGame()
        {
            var message = JsonSerializer.Serialize(new ChallengeMessage(_myName, OpponentName));
            Inject.Application.Server.CreateAndSendPacket(OpCodes.FinishedSetup, message);
            Inject.Application.MySetShips = ShipGrid.Ships.ToList();
            WaitingMessageVisible = true;

            if (_opponentFinished)
            {
                GoToBattle();
            }
        }

        #endregion

        #region Methods

        private void GoToBattle()
        {
            WaitingMessageVisible = false;
            Inject.Application.CurrentPage = ApplicationPages.BattlePage;
        }

        #endregion
    }
}
