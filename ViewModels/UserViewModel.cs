using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships;

internal partial class UserViewModel : ObservableObject
{
	#region Properties

	[ObservableProperty]
	private string name;

	[ObservableProperty]
	private bool isBusy;

	#endregion

	#region Constructor

	public UserViewModel()
	{

	} 

	#endregion
}
