using BattleshipServer.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships;
public class SignalRServer
{
    #region Fields

    private HubConnection _hubConnection;

    #endregion

    #region Properties

    public string ConnectionId => _hubConnection.ConnectionId;

    #endregion

    #region Actions

    public Action<string> DisconnectedClientAction { get; internal set; }
    public Action<User> NewUserAction { get; internal set; }
    public Action<string> ChallengePlayerAction { get; internal set; }
    public Action<string> ChallengeAnswerAction { get; internal set; }
    public Action<string> BusyAction { get; internal set; }
    public Action<List<User>> UpdateUserListAction { get; internal set; }

    #endregion
    
    #region Methods

    internal async void ConnectToServer()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7012/battleships")
            .WithAutomaticReconnect()
            .Build();

        DefineHubReceiveMethods();

        await _hubConnection.StartAsync();
    }

    private void DefineHubReceiveMethods()
    {
        _hubConnection.On<User>("ReceiveNewUser", user => NewUserAction?.Invoke(user));
        _hubConnection.On<List<User>>("ReceiveUserList", list => UpdateUserListAction?.Invoke(list));
    }

    #endregion

    #region Send to hub methods

    internal async Task SendNewUserAsync(User user)
    {
        if(_hubConnection != null)
        {
            user.ConnectionId = ConnectionId;
            await _hubConnection.SendAsync("SendNewUser", user);
        }
    }

    #endregion
}
