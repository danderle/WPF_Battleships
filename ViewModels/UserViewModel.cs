using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships;

internal partial class UserViewModel : ObservableObject
{
	[ObservableProperty]
	private string name;

	public UserViewModel()
	{

	}
}
