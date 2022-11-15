using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships;

public partial class ShipViewModel : ObservableObject
{
	#region Properties

	[ObservableProperty]
	private double width;

	[ObservableProperty]
	private double xpos = 0;

    [ObservableProperty]
    private double ypos = 0;

    [ObservableProperty]
	private ShipTypes shipType;

	#endregion

	#region Constructor

	public ShipViewModel()
	{

	}

	public ShipViewModel(ShipTypes shipType)
	{
        ShipType = shipType;
		Width = 120;
	}

	#endregion
}
