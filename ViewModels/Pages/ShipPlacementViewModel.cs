﻿using BattleshipServer;
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
        private bool waitingMessageVisible;
        
        [ObservableProperty]
        private ShipGridViewModel shipGrid = new ShipGridViewModel();

        public string OpponentName => Inject.Application.OpponentName;

        #endregion

        #region Constructor

        public ShipPlacementViewModel()
        {
            Inject.Application.Server.FinishedSetupAction = FinishedSetup;
        }

        #endregion

        #region Server Actions

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
            Inject.Application.CurrentPage = ApplicationPages.BattlePage;
        }

        #endregion
    }
}