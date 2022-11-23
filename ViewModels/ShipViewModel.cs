using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Battleships;

public partial class ShipViewModel : ObservableObject
{
	#region Properties

	private readonly int SingleSquareSize = 40;

	private int _size = 0;

	[ObservableProperty]
	private double width;

	[ObservableProperty]
	private double xpos = 0;

    [ObservableProperty]
    private double ypos = 0;

    [ObservableProperty]
	private ShipTypes shipType;

    [ObservableProperty]
    private ShipAlignment alignment;


    #endregion

    #region Constructor

    public ShipViewModel()
	{

	}

	public ShipViewModel(ShipTypes shipType)
	{
        ShipType = shipType;
		ShipSetup(shipType);
	}

	private void ShipSetup(ShipTypes shipType)
	{
		switch (shipType)
		{
			case ShipTypes.Destroyer:
                _size = 2;
				break;
            case ShipTypes.Submarine:
                _size = 3;
                break;
            case ShipTypes.Cruiser:
                _size = 3;
                break;
            case ShipTypes.Battleship:
                _size = 4;
                break;
            case ShipTypes.Carrier:
                _size = 5;
                break;
        }

		Width = SingleSquareSize * _size;
	}

	#endregion
}
