using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Battleships;

public partial class ShipViewModel : ObservableObject
{
    #region Fields

    private readonly int SingleSquareSize = 40;

    private int _size = 0;
    private int _hitCounter = 0;

    private Coordinate _resetCoordinate;

    #endregion

    #region Properties

    [ObservableProperty]
    private bool isDestroyed;

    [ObservableProperty]
	private double width;

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

    public double Height => SingleSquareSize;
    public ObservableCollection<Coordinate> HitCoordinates { get; set; } = new ObservableCollection<Coordinate>();

    #endregion

    #region Constructor

    public ShipViewModel()
	{

	}

	public ShipViewModel(ShipTypes shipType, double yPos)
	{
        ShipType = shipType;
        Ypos = yPos;
		ShipSetup();
	}

    public ShipViewModel(ShipViewModel ship)
    {
        ShipType = ship.ShipType;
        Alignment = ship.Alignment;
        Angle = ship.Angle;
        Xpos = ship.Xpos;
        Ypos = ship.Ypos;
        ShipSetup();
    }

    public ShipViewModel(ShipDestroyedMessage ship)
    {
        ShipType = ship.ShipType;
        Alignment = ship.Alignment;
        Angle = Alignment == ShipAlignment.Horizontal ? 0 : 90;
        IsDestroyed = true;
        Xpos = ship.Xpos;
        Ypos = ship.Ypos;
        ShipSetup();
    }

    #endregion

    #region Commands

    [RelayCommand]
    public void Rotate()
    {
        _resetCoordinate = new Coordinate(Xpos, Ypos);

        switch(Alignment)
        {
            case ShipAlignment.Horizontal:
                Alignment = ShipAlignment.Vertical;
                Angle = 90;
                if (Ypos + Width >= 400)
                {
                    Ypos -= (Ypos + Width) - 400;
                }
                break;
            case ShipAlignment.Vertical:
                Alignment = ShipAlignment.Horizontal;
                Angle = 0;
                if (Xpos + Width >= 400)
                {
                    Xpos -= (Xpos + Width) - 400;
                }
                break;
        }

        SetHitCoordinates();
    }

    #endregion

    #region Public methods

    public void SetHitCoordinates()
    {
        HitCoordinates.Clear();

        switch (Alignment)
        {
            case ShipAlignment.Horizontal:
                for (double x = Xpos; x < Xpos + Width; x += SingleSquareSize)
                {
                    HitCoordinates.Add(new Coordinate(x, Ypos));
                }
                break;
            case ShipAlignment.Vertical:
                for (double y = Ypos; y < Ypos + Width; y += SingleSquareSize)
                {
                    HitCoordinates.Add(new Coordinate(Xpos, y));
                }
                break;
        }
    }

    internal void Reset()
    {
        Xpos = _resetCoordinate.Xpos;
        Ypos = _resetCoordinate.Ypos;
        Rotate();
    }

    internal void Hit()
    {
        _hitCounter++;
        IsDestroyed = _hitCounter >= _size;
    }

    #endregion

    #region Methods

    private void ShipSetup()
    {
        switch (ShipType)
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
        SetHitCoordinates();
    }

    #endregion
}
