using BattleshipServer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Data;

namespace Battleships;

internal partial class MainMenuViewModel : ObservableObject
{
	#region Fields

	private object _lock = new object();
	private ClientToServer _clientToServer = new ClientToServer();

	#endregion

	#region Properties

	[ObservableProperty]
	private string username;

    [ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ChallengeCommand))]
    private UserViewModel opponent;

    [ObservableProperty]
    private bool connected;

    [ObservableProperty]
    private bool challenged;

    public ObservableCollection<UserViewModel> Users { get; set; } = new ObservableCollection<UserViewModel>();


	#endregion

	#region Constructor

	public MainMenuViewModel()
	{
		BindingOperations.EnableCollectionSynchronization(Users, _lock);
		_clientToServer.ConnectedAction = NewConnection;
		_clientToServer.ChallengePlayerAction = ChallengedByPlayer;
    }

	#endregion

	#region Server actions

	private void NewConnection(string newUsername)
	{
		if (!Users.Any(user => user.Name == newUsername))
		{
			var user = new UserViewModel()
			{
				Name = newUsername,
			};

			lock (_lock)
			{
				Users.Add(user);
			}
		}

		if (Username == newUsername)
		{
			Connected = true;
		}
	}

	private void ChallengedByPlayer(string message)
	{
		ChallengeMessage msg = JsonSerializer.Deserialize<ChallengeMessage>(message);
		Challenged = true;
		Opponent = Users.FirstOrDefault(user => user.Name == msg.Challenger);
    }

    #endregion

    #region Command methods

    [RelayCommand(CanExecute = nameof(CanConnectToServer))]
	private void ConnectToServer()
	{
		_clientToServer.Username = username;
		_clientToServer.ConnectToServer();
	}

	private bool CanConnectToServer()
	{
		return !string.IsNullOrEmpty(Username);
	}

	[RelayCommand(CanExecute = nameof(CanChallenge))]
    private void Challenge()
    {
		var message = JsonSerializer.Serialize(new ChallengeMessage(Username, Opponent.Name));
		_clientToServer.CreateAndSendPacket(OpCodes.ChallengePlayer, message);
    }

	private bool CanChallenge()
	{
		return Opponent != null && Opponent.Name != Username;
	}

	[RelayCommand]
    private void Accept()
    {
    }

    [RelayCommand]
    private void Deny()
    {
    }

    #endregion
}
