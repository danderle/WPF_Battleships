using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships;

public partial class UserViewModel : ObservableObject
{
    #region Properties
    public string ConnectionId { get; set; }

    [ObservableProperty]
	private string name;

	[ObservableProperty]
	private bool isBusy;

	#endregion
}
