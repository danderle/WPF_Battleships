using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Media;

namespace Battleships;

public partial class ShipViewModel : ObservableObject
{
	#region Properties

	private readonly int SingleSquareSize = 40;

	private int _size = 0;

	[ObservableProperty]
	private double width;

    [ObservableProperty]
    private double height;

    [ObservableProperty]
	private double xpos = 0;

    [ObservableProperty]
    private double ypos = 0;

    [ObservableProperty]
    private double angle = 0;

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

    #endregion

    #region Commands

    [RelayCommand]
    public void Rotate()
    {
        if (Alignment == ShipAlignment.Horizontal)
        {
            Alignment = ShipAlignment.Vertical;
            Angle = 90;
        }
        else
        {
            Alignment = ShipAlignment.Horizontal;
            Angle = 0;
        }
    }

    #endregion

    #region Methods

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
        Height = 40;
    }

    #endregion
}
