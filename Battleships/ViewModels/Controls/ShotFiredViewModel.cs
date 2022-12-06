using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships;

public partial class ShotFiredViewModel : ObservableObject
{
    #region Properties

    [ObservableProperty]
    private bool hit;

    [ObservableProperty]
	private Coordinate coordinate;

	public ShotFiredViewModel(ShotFiredMessage shot)
	{
		Coordinate = new Coordinate(shot.Xpos, shot.Ypos);
		Hit = shot.Hit;
	}

	#endregion

	#region Constructor

	public ShotFiredViewModel(double xSnap, double ySnap, bool isHit = false)
	{
		coordinate = new Coordinate(xSnap, ySnap);
		Hit = isHit;
	} 

	#endregion
}
