using BattleshipServer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace Battleships;

public partial class HitMarkerViewModel : ObservableObject
{
	#region Properties

	public string OpponentName => Inject.Application.OpponentName;
	public ObservableCollection<ShotFiredViewModel> ShotsFired { get; set; } = new ObservableCollection<ShotFiredViewModel>();

	#endregion

	#region Constructor

	public HitMarkerViewModel()
	{
		Inject.Application.Server.ShotFiredAction = ShotFired;
	}

    #endregion

    #region Server actions

    private void ShotFired(string message)
    {
        var shot = JsonSerializer.Deserialize<ShotFiredMessage>(message);
        ShotsFired.Add(new ShotFiredViewModel(shot));
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
}
