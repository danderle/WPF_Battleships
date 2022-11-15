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
		Ships.Add(new ShipViewModel(ShipTypes.Submarine));
	} 

	#endregion
}
