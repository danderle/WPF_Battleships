﻿using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Threading.Tasks;

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
        public UserViewModel Opponent => Inject.Application.Opponent;

        #endregion

        #region Constructor

        public ShipPlacementViewModel()
        {
            Inject.Application.Server.DisconnectedClientAction = DisconnectedClientAction;
            Inject.Application.Server.FinishedSetupAction = FinishedSetup;

            Inject.Application.SignalR.ReceiveFinishedSetupAction = ReceiveFinishedSetup;
            Inject.Application.SignalR.ReceiveUserDisconnectedAction = ReceiveUserDisconnected;

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

        #region SignalR actions

        private void ReceiveUserDisconnected(string connectionId)
        {
            if (Opponent.ConnectionId == connectionId)
            {
                OpponentDisconnected = true;
            }
        }

        public void ReceiveFinishedSetup(string message)
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
        public async Task Continue()
        {
            var user = new User()
            {
                Name = _myName,
                IsBusy = false,
                Starts = false
            };

            //string message = JsonSerializer.Serialize(user);
            //Inject.Application.Server.CreateAndSendPacket(OpCodes.Busy, message);
            await Inject.Application.SignalR.SendUserUpdate(user);
            Inject.Application.CurrentPage = ApplicationPages.MainMenuPage;
        }

        [RelayCommand]
        public async Task StartGame()
        {
            //var message = JsonSerializer.Serialize(new ChallengeMessage(_myName, OpponentName));
            //Inject.Application.Server.CreateAndSendPacket(OpCodes.FinishedSetup, message);

            await Inject.Application.SignalR.SendFinishedSetup(Opponent.ConnectionId);
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
