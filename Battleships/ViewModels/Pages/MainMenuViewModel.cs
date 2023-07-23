using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace Battleships;

internal partial class MainMenuViewModel : ObservableObject
{
	#region Fields

	private object _lock = new object();

    #endregion

    #region Properties

    [ObservableProperty]
    private bool opponentDisconnected;

    [ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(CreateNewUserCommand))]
	private string username;

    [ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ChallengeCommand))]
    private UserViewModel opponent;

    [ObservableProperty]
    private string opponentName;

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
        Inject.Application.Server.DisconnectedClientAction = DisconnectedClientAction;
        Inject.Application.Server.NewUserAction = NewConnection;
		Inject.Application.Server.ChallengePlayerAction = ChallengedByPlayer;
		Inject.Application.Server.ChallengeAnswerAction = ChallengeAnswer;
        Inject.Application.Server.BusyAction = Busy;
        Inject.Application.Server.UpdateUserListAction = UpdateUserListAction;
        //Inject.Application.Server.ConnectToServer();

        Inject.Application.SignalR.ConnectToServer();
        Inject.Application.SignalR.ReceiveNewUserAction = NewUser;
        Inject.Application.SignalR.ReceiveUpdateUserListAction = UpdateUserList;
        Inject.Application.SignalR.ReceiveChallengeAction = ReceivedChallenge;
        Inject.Application.SignalR.ReceiveUserUpdateAction = ReceiveUserUpdate;
        Inject.Application.SignalR.ReceiveChallengeAnswerAction = ReceiveChallengeAnswer;

        Inject.Application.SignalR.DisconnectedClientAction = DisconnectedClientAction;
    }

    #endregion

    #region Server actions

    private void UpdateUserListAction(string userList)
    {
		var list = JsonSerializer.Deserialize<List<User>>(userList);

		for (int index = Users.Count - 1; index >= 0; index--)
		{
			var user = list.FirstOrDefault(item => item.Name == Users[index].Name);
			if (user == null)
			{
				Users.RemoveAt(index);
			}
		}
    }

    private void DisconnectedClientAction(string disconnectedUsername)
    {
		var user = Users.FirstOrDefault(item => item.Name == disconnectedUsername);
		if (user != null)
		{
			Users.Remove(user);
        }

        if (Opponent != null && Opponent.Name == disconnectedUsername)
        {
			OpponentName = Opponent.Name;
            OpponentDisconnected = true;
        }
    }

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
			Inject.Application.MyName = Username;
		}
	}

	private void ChallengedByPlayer(string message)
	{
		ChallengeMessage msg = JsonSerializer.Deserialize<ChallengeMessage>(message);
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
			OpenChallenge = false;
			GoToPlacementPage();
        }
        else
		{
			DeniedChallenge = true;
		}

		WaitingForChallengeAnswer = false;
    }

    #endregion

    #region SignalR actions

    private async void ReceiveChallengeAnswer(bool answer)
    {
        if (answer)
        {
            OpenChallenge = false;
            GoToPlacementPage();
        }
        else
        {
            DeniedChallenge = true;
            var userUpdate = new User
            {
                IsBusy = false
            };

            await Inject.Application.SignalR.SendUserUpdate(userUpdate);
        }

        WaitingForChallengeAnswer = false;
    }

    private void ReceiveUserUpdate(User user)
    {
        var update = Users.FirstOrDefault(item => item.ConnectionId == user.ConnectionId);
        update.IsBusy = user.IsBusy;
    }

    private async void ReceivedChallenge(string challengerId)
    {
        await Application.Current.Dispatcher.BeginInvoke(() =>
        {
            Opponent = Users.FirstOrDefault(user => user.ConnectionId == challengerId);
            Opponent.IsBusy = true;
        });

        Challenged = true;
        Users.FirstOrDefault(user => user.Name == Username).IsBusy = true;
        var userUpdate = new User()
        {
            IsBusy = true
        };

        await Inject.Application.SignalR.SendUserUpdate(userUpdate);
    }

    private void NewUser(User newUser)
    {
        if (!Users.Any(user => user.ConnectionId == newUser.ConnectionId))
        {
            var user = new UserViewModel()
            {
                Name = newUser.Name,
                ConnectionId = newUser.ConnectionId
            };

            lock (_lock)
            {
                Users.Add(user);
            }
        }

        if (Username == newUser.Name)
        {
            Connected = true;
            Inject.Application.MyName = Username;
        }
    }


    private void UpdateUserList(List<User> list)
    {
        for (int index = Users.Count - 1; index >= 0; index--)
        {
            var user = list.FirstOrDefault(item => item.ConnectionId == Users[index].ConnectionId);
            if (user == null)
            {
                Users.RemoveAt(index);
            }
        }
    }

    #endregion

    #region Command methods

    [RelayCommand]
    public void Continue()
    {
        var user = new User()
		{
            Name = Username,
            IsBusy = false,
            Starts = false
        };

        string message = JsonSerializer.Serialize(user);
        Inject.Application.Server.CreateAndSendPacket(OpCodes.Busy, message);
		Challenged = false;
		OpenChallenge = false;
		OpponentDisconnected = false;
    }

    [RelayCommand(CanExecute = nameof(CanCreateNewUser))]
	private async Task CreateNewUser()
	{
		//Inject.Application.Server.CreateAndSendPacket(OpCodes.NewUser, Username);
		//Inject.Application.Server.Username = Username;

		var user = new User
		{
			Name = Username,
		};

		await Inject.Application.SignalR.SendNewUserAsync(user);
	}

	private bool CanCreateNewUser()
	{
		return !string.IsNullOrWhiteSpace(Username) && !Users.Any(item => item.Name == Username);
	}

	[RelayCommand(CanExecute = nameof(CanChallenge))]
    private async Task Challenge()
    {
        //var message = JsonSerializer.Serialize(new ChallengeMessage(Username, Opponent.Name));
        //      Inject.Application.Server.CreateAndSendPacket(OpCodes.ChallengePlayer, message);

        await Inject.Application.SignalR.SendChallenge(Opponent.ConnectionId);

		WaitingForChallengeAnswer = true;
		OpenChallenge = true;
     
		var user = Users.FirstOrDefault(user => user.Name == Username);
		user.IsBusy = true;

		var userUpdate = new User()
		{
			Name = user.Name,
            ConnectionId = user.ConnectionId,
			IsBusy = user.IsBusy
		};

        await Inject.Application.SignalR.SendUserUpdate(userUpdate);

		//message = JsonSerializer.Serialize(u);
  //      Inject.Application.Server.CreateAndSendPacket(OpCodes.Busy, message);
    }

    private bool CanChallenge()
	{
		return Opponent != null && Opponent.Name != Username && !Opponent.IsBusy;
	}

	[RelayCommand]
    private async Task Accept()
    {
        //var msg = JsonSerializer.Serialize(new ChallengeAnswerMessage(Opponent.Name, Username, true));
        //      Inject.Application.Server.CreateAndSendPacket(OpCodes.ChallengeAnswer, msg);

        await Inject.Application.SignalR.SendChallengeAnswer(Opponent.ConnectionId, true);
		GoToPlacementPage();
		Challenged = false;
    }

    [RelayCommand]
    private async Task Deny()
    {
        //var msg = JsonSerializer.Serialize(new ChallengeAnswerMessage(Opponent.Name, Username, false));
        //Inject.Application.Server.CreateAndSendPacket(OpCodes.ChallengeAnswer, msg);
        await Inject.Application.SignalR.SendChallengeAnswer(Opponent.ConnectionId, false);
        var userUpdate = new User
        {
            IsBusy = false
        };
        
        await Inject.Application.SignalR.SendUserUpdate(userUpdate);
        Challenged = false;
    }

    [RelayCommand]
    private void Ok()
    {
		DeniedChallenge = false;
		OpenChallenge = false;
    }

    #endregion

    #region Methods

    private void GoToPlacementPage()
	{
		Inject.Application.OpponentName = Opponent.Name;
        Inject.Application.CurrentPage = ApplicationPages.ShipPlacementPage;
    }

    internal void UpdateUserList()
    {
		if (!string.IsNullOrEmpty(Username))
		{
			Inject.Application.Server.CreateAndSendPacket(OpCodes.UpdateUserList, Username);
		}
    }
    #endregion
}
