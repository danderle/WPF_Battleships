﻿using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Battleships
{
    public partial class BattleViewModel : ObservableObject
    {
        #region Fields

        #endregion

        #region Properties

        [ObservableProperty]
        private bool gameOver;

        [ObservableProperty]
        private bool winner;

        [ObservableProperty]
        private bool myTurn;

        [ObservableProperty]
        private ShipGridViewModel myShipGrid = new();

        [ObservableProperty]
        private ShipGridViewModel enemyShipGrid = new();

        [ObservableProperty]
        private HitMarkerViewModel myHitGrid = new HitMarkerViewModel();

        [ObservableProperty]
        private HitMarkerViewModel enemyHitGrid = new HitMarkerViewModel();

        public string MyName => Inject.Application.MyName;
        public string OpponentName => Inject.Application.OpponentName;

        #endregion

        #region Constructor

        public BattleViewModel()
        {
            Inject.Application.Server.GameoverAction = GameOverAction;
            MyHitGrid.SwitchTurn = SwitchTurn;
            EnemyHitGrid.SwitchTurn = SwitchTurn;
            Inject.Application.Server.ShotFiredAction = MyHitGrid.ShotFired;
            myShipGrid.Ships = new ObservableCollection<ShipViewModel>(Inject.Application.MySetShips);
            MyTurn = Inject.Application.Istart;
        }
        #endregion

        #region Server Actions

        private void GameOverAction(string message)
        {
            var gameOver = JsonSerializer.Deserialize<GameOverMessage>(message);

            GameOver = true;
            if (gameOver.Winner == MyName)
            {
                Winner = true;
            }
        }

        private void SwitchTurn()
        {
            MyTurn = !MyTurn;
        }

        #endregion

        #region Command method

        [RelayCommand]
        public void Continue()
        {
            var user = new User()
            {
                Name = MyName,
                IsBusy = false,
            };

            string message = JsonSerializer.Serialize(user);
            Inject.Application.Server.CreateAndSendPacket(OpCodes.Busy, message);
            Inject.Application.CurrentPage = ApplicationPages.MainMenuPage;
        }

        #endregion

        #region Methods


        #endregion
    }
}
