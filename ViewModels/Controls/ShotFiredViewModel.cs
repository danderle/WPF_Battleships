using BattleshipServer;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships;

public partial class ShotFiredViewModel : ObservableObject
{
	#region Properties

	[ObservableProperty]
	private Coordinate coordinate;

	public ShotFiredViewModel(ShotFiredMessage shot)
	{
		Coordinate = new Coordinate(shot.Xpos, shot.Ypos);
	}

	#endregion

	#region Constructor

	public ShotFiredViewModel(double xSnap, double ySnap)
	{
		coordinate = new Coordinate(xSnap, ySnap);
	} 

	#endregion
}
