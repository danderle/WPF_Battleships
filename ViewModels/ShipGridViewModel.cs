using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Battleships;

public partial class ShipGridViewModel : ObservableObject
{
	#region Properties

	public ObservableCollection<ShipViewModel> Ships { get; set; } = new ObservableCollection<ShipViewModel>();

    #endregion

    #region Constructor

    public ShipGridViewModel()
	{
		Ships.Add(new ShipViewModel(ShipTypes.Carrier) { Ypos = 0});
		Ships.Add(new ShipViewModel(ShipTypes.Battleship) { Ypos = 40});
		Ships.Add(new ShipViewModel(ShipTypes.Cruiser) { Ypos = 80});
		Ships.Add(new ShipViewModel(ShipTypes.Submarine) { Ypos = 120});
		Ships.Add(new ShipViewModel(ShipTypes.Destroyer) { Ypos = 160});
    } 

	#endregion
}
