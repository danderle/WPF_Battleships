using BattleshipServer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace Battleships;

public partial class HitMarkerViewModel : ObservableObject
{
	#region Fields

	private List<ShipViewModel> _placedShips => Inject.Application.MySetShips.ToList();

	#endregion

	#region Properties

	public string OpponentName => Inject.Application.OpponentName;
	public ObservableCollection<ShotFiredViewModel> ShotsFired { get; set; } = new ObservableCollection<ShotFiredViewModel>();

	#endregion

	#region Constructor

	public HitMarkerViewModel()
	{
		Inject.Application.Server.ShotFiredAction = ShotFired;
		Inject.Application.Server.ShotConfirmationAction = ShotConfirmation;
    }

	#endregion

	#region Server actions

	private void ShotFired(string message)
    {
        var shot = JsonSerializer.Deserialize<ShotFiredMessage>(message);
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
					shot.Opponent = OpponentName;
					message = JsonSerializer.Serialize(shot);
					Inject.Application.Server.CreateAndSendPacket(OpCodes.ShotConfirmation, message);

					if (ship.IsDestroyed)
					{
						message = JsonSerializer.Serialize(new ShipDestroyedMessage(OpponentName, ship.ShipType, ship.Alignment, ship.Xpos, ship.Ypos));
						Inject.Application.Server.CreateAndSendPacket(OpCodes.ShipDestroyed, message);
                    }
				}
			}
		}

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
			//todo switch turn
		}
    }


    #endregion

    #region Command methods

    [RelayCommand]
	public void Fire(ShotFiredViewModel shot)
	{
		if (!ShotsFired.Any(item => item.Coordinate.Compare(shot.Coordinate)))
		{
			var shotFired = new ShotFiredMessage(OpponentName, shot.Coordinate.Xpos, shot.Coordinate.Ypos);
			var message = JsonSerializer.Serialize(shotFired);
			ShotsFired.Add(shot);
			Inject.Application.Server.CreateAndSendPacket(OpCodes.ShotFired, message);
		}
	}

    #endregion

    #region Private methods

    private void AddShotFired(ShotFiredMessage shot)
    {
        ShotsFired.Add(new ShotFiredViewModel(shot));
    }

    #endregion
}
