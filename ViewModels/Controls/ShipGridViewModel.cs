using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace Battleships;

public partial class ShipGridViewModel : ObservableObject
{
	#region Properties

	public ObservableCollection<ShipViewModel> Ships { get; set; } = new ObservableCollection<ShipViewModel>();

    #endregion

    #region Constructor

    public ShipGridViewModel()
	{
		Ships.Add(new ShipViewModel(ShipTypes.Carrier, 0));
		Ships.Add(new ShipViewModel(ShipTypes.Battleship, 40));
		Ships.Add(new ShipViewModel(ShipTypes.Cruiser, 80));
		Ships.Add(new ShipViewModel(ShipTypes.Submarine, 120));
		Ships.Add(new ShipViewModel(ShipTypes.Destroyer, 160));
    }

	#endregion

	#region Command methods

	[RelayCommand]
	public void CheckIfOverlapping(ShipViewModel testShip)
	{
		if (!IsOverlapping(testShip))
		{
			var setShip = Ships.FirstOrDefault(item => item.ShipType == testShip.ShipType);
			setShip.Xpos = testShip.Xpos;
			setShip.Ypos = testShip.Ypos;
			setShip.HitCoordinates = testShip.HitCoordinates;
        }
	}

    [RelayCommand]
    public void Rotate(ShipViewModel ship)
    {
        ship.Rotate();
        if (IsOverlapping(ship))
        {
            ship.Reset();
        }
    }

    #endregion

    #region Methods

    private bool IsOverlapping(ShipViewModel testShip)
    {
        foreach (var ship in Ships)
        {
            if (ship.ShipType != testShip.ShipType)
            {
                foreach (var coo in ship.HitCoordinates)
                {
                    foreach (var testCoo in testShip.HitCoordinates)
                    {
                        if (coo.Compare(testCoo))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    #endregion
}
