using BattleshipServer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
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
	[NotifyCanExecuteChangedFor(nameof(ConnectToServerCommand))]
	private string username;

    [ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ChallengeCommand))]
    private UserViewModel opponent;

    [ObservableProperty]
    private bool connected;

    [ObservableProperty]
    private bool challenged;

    [ObservableProperty]
    private bool waitingForChallengeAnswer;

    [ObservableProperty]
    private bool deniedChallenge;

    [ObservableProperty]
    private bool openChallenge;

    public ObservableCollection<UserViewModel> Users { get; set; } = new ObservableCollection<UserViewModel>();


	#endregion

	#region Constructor

	public MainMenuViewModel()
	{
		BindingOperations.EnableCollectionSynchronization(Users, _lock);
		_clientToServer.ConnectedAction = NewConnection;
		_clientToServer.ChallengePlayerAction = ChallengedByPlayer;
		_clientToServer.ChallengeAnswerAction = ChallengeAnswer;
		_clientToServer.BusyAction = Busy;
    }
	#endregion

	#region Server actions


	private void Busy(string message)
	{
		User user = JsonSerializer.Deserialize<User>(message);
		var update = Users.FirstOrDefault(item => item.Name == user.Name);
		update.IsBusy = user.IsBusy;
	}

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
		Application.Current.Dispatcher.BeginInvoke(() =>
		{
			Opponent = Users.FirstOrDefault(user => user.Name == msg.Challenger);
			Opponent.IsBusy = true;
		});

		Challenged = true;
		Users.FirstOrDefault(user => user.Name == Username).IsBusy = true;
    }

    private void ChallengeAnswer(string message)
    {
		ChallengeAnswerMessage msg = JsonSerializer.Deserialize<ChallengeAnswerMessage>(message);

		if (msg.Accept)
		{
			Opponent = Users.FirstOrDefault( user => user.Name == msg.Defender);
            Inject.Application.CurrentPage = ApplicationPages.ShipPlacementPage;
        }
        else
		{
			DeniedChallenge = true;
		}

		WaitingForChallengeAnswer = false;
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
		WaitingForChallengeAnswer = true;
		OpenChallenge = true;
     
		var user = Users.FirstOrDefault(user => user.Name == Username);
		user.IsBusy = true;

		var u = new User()
		{
			Name = user.Name,
			IsBusy = user.IsBusy
		};

		message = JsonSerializer.Serialize(u);
		_clientToServer.CreateAndSendPacket(OpCodes.Busy, message);
    }

    private bool CanChallenge()
	{
		return Opponent != null && Opponent.Name != Username && !Opponent.IsBusy;
	}

	[RelayCommand]
    private void Accept()
    {
		var msg = JsonSerializer.Serialize(new ChallengeAnswerMessage(Opponent.Name, Username, true));
		_clientToServer.CreateAndSendPacket(OpCodes.ChallengeAnswer, msg);

		Inject.Application.CurrentPage = ApplicationPages.ShipPlacementPage;
    }

    [RelayCommand]
    private void Deny()
    {
        var msg = JsonSerializer.Serialize(new ChallengeAnswerMessage(Opponent.Name, Username, false));
        _clientToServer.CreateAndSendPacket(OpCodes.ChallengeAnswer, msg);
		Challenged = false;
    }

    [RelayCommand]
    private void Ok()
    {
		DeniedChallenge = false;
		OpenChallenge = false;
    }

    #endregion
}
