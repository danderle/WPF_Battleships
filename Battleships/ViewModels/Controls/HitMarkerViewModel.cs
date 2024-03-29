﻿using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace Battleships;

public partial class HitMarkerViewModel : ObservableObject
{
    #region Fields

    private object _lock = new object();

	private int _shipsDestroyed;
	private bool _gameover;

    private List<ShipViewModel> _placedShips => Inject.Application.MySetShips.ToList();

	#endregion

	#region Properties

	public string OpponentName => Inject.Application.OpponentName;
	public string MyName => Inject.Application.MyName;
	public ObservableCollection<ShotFiredViewModel> ShotsFired { get; set; } = new ObservableCollection<ShotFiredViewModel>();

	public Action SwitchTurn;

	#endregion

	#region Constructor

	public HitMarkerViewModel()
	{
		BindingOperations.EnableCollectionSynchronization(ShotsFired, _lock);
		Inject.Application.Server.ShotConfirmationAction = ShotConfirmation;

		Inject.Application.SignalR.ReceiveShotConfirmationAction = ReceiveShotConfirmation;
    }

	#endregion

	#region Server actions

	public void ShotFired(string message)
    {
        var shot = JsonSerializer.Deserialize<ShotFiredMessage>(message);
		shot.Opponent = OpponentName;
		foreach (var ship in _placedShips)
        {
			if (!ship.IsDestroyed)
			{
				var shotCoo = new Coordinate(shot.Xpos, shot.Ypos);
				var hitCoo = ship.HitCoordinates.FirstOrDefault(item => item.Compare(shotCoo));
				if (hitCoo != null)
				{
					shot.Hit = true;
					ship.Hit();
					AddShotFired(shot);
					message = JsonSerializer.Serialize(shot);
					Inject.Application.Server.CreateAndSendPacket(OpCodes.ShotConfirmation, message);

					if (ship.IsDestroyed)
					{
						_shipsDestroyed++;
						message = JsonSerializer.Serialize(new ShipDestroyedMessage(OpponentName, ship.ShipType, ship.Alignment, ship.Xpos, ship.Ypos));
						Inject.Application.Server.CreateAndSendPacket(OpCodes.ShipDestroyed, message);

						_gameover = _placedShips.Count == _shipsDestroyed;
						if (_gameover)
						{
							message = JsonSerializer.Serialize(new GameOverMessage(OpponentName, MyName));
							Inject.Application.Server.CreateAndSendPacket(OpCodes.GameOver, message);
						}
                    }

					return;
				}
			}
		}

		AddShotFired(shot);
		message = JsonSerializer.Serialize(shot);
		Inject.Application.Server.CreateAndSendPacket(OpCodes.ShotConfirmation, message);
		SwitchTurn?.Invoke();
    }

    private void ShotConfirmation(string message)
    {
		var shot = JsonSerializer.Deserialize<ShotFiredMessage>(message);
		if (shot.Hit)
		{
			ShotsFired.Last().Hit = true;
		}
		else
		{
			SwitchTurn?.Invoke();
		}
    }

    #endregion

    #region SignalR actions

    private void ReceiveShotConfirmation(ShotFiredMessage shot)
    {
        if (shot.Hit)
        {
            ShotsFired.Last().Hit = true;
        }
        else
        {
            SwitchTurn?.Invoke();
        }
    }

    public async void ReceiveShotFired(ShotFiredMessage shot)
    {
        shot.Opponent = OpponentName;
        foreach (var ship in _placedShips)
        {
            if (!ship.IsDestroyed)
            {
                var shotCoo = new Coordinate(shot.Xpos, shot.Ypos);
                var hitCoo = ship.HitCoordinates.FirstOrDefault(item => item.Compare(shotCoo));
                if (hitCoo != null)
                {
                    shot.Hit = true;
                    ship.Hit();
                    AddShotFired(shot);
					await Inject.Application.SignalR.SendShotConfirmation(shot);

                    if (ship.IsDestroyed)
                    {
                        _shipsDestroyed++;
                        var message = new ShipDestroyedMessage(OpponentName, ship.ShipType, ship.Alignment, ship.Xpos, ship.Ypos);
                        await Inject.Application.SignalR.SendShipDestroyed(message);

                        _gameover = _placedShips.Count == _shipsDestroyed;
                        if (_gameover)
                        {
                            var gameoverMessage = new GameOverMessage(Inject.Application.Opponent.ConnectionId, Inject.Application.SignalR.ConnectionId);
							await Inject.Application.SignalR.SendGameoverMessage(gameoverMessage);
                        }
                    }

                    return;
                }
            }
        }

        AddShotFired(shot);
		await Inject.Application.SignalR.SendShotConfirmation(shot);
        SwitchTurn?.Invoke();
    }
    

    #endregion

    #region Command methods

    [RelayCommand]
	public async Task Fire(ShotFiredViewModel shot)
	{
		if (!ShotsFired.Any(item => item.Coordinate.Compare(shot.Coordinate)))
		{
			var shotFired = new ShotFiredMessage(
				shot.Coordinate.Xpos, shot.Coordinate.Ypos, Inject.Application.Opponent.ConnectionId, Inject.Application.SignalR.ConnectionId);
			ShotsFired.Add(shot);
			await Inject.Application.SignalR.SendShotFired(shotFired);
		}
	}

    #endregion

    #region Private methods

    private void AddShotFired(ShotFiredMessage shot)
    {
		lock (_lock)
		{
			ShotsFired.Add(new ShotFiredViewModel(shot));
		}
    }

    #endregion
}
