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
    public Action<User> ReceiveNewUserAction { get; internal set; }
    public Action<string> ReceiveChallengeAction { get; internal set; }
    public Action<bool> ReceiveChallengeAnswerAction { get; internal set; }
    public Action<User> ReceiveUserUpdateAction { get; internal set; }
    public Action<List<User>> ReceiveUpdateUserListAction { get; internal set; }
    public Action<string> ReceiveFinishedSetupAction { get; internal set; }
    public Action<bool> ReceiveWhoStartsAction { get; internal set; }

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
        _hubConnection.On<User>("ReceiveNewUser", user => ReceiveNewUserAction?.Invoke(user));
        _hubConnection.On<List<User>>("ReceiveUserList", list => ReceiveUpdateUserListAction?.Invoke(list));
        _hubConnection.On<string>("ReceiveChallenge", connectionId => ReceiveChallengeAction?.Invoke(connectionId));
        _hubConnection.On<User>("ReceiveUserUpdate", user => ReceiveUserUpdateAction?.Invoke(user));
        _hubConnection.On<bool>("ReceiveChallengeAnswer", answer => ReceiveChallengeAnswerAction?.Invoke(answer));
        _hubConnection.On<string>("ReceiveFinishedSetup", answer => ReceiveFinishedSetupAction?.Invoke(answer));
        _hubConnection.On<bool>("ReceiveWhoStarts", answer => ReceiveWhoStartsAction?.Invoke(answer));
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

    internal async Task SendChallenge(string opponentConnectionId)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendChallenge", opponentConnectionId, ConnectionId);
        }
    }

    internal async Task SendUserUpdate(User userUpdate)
    {
        if (_hubConnection != null)
        {
            userUpdate.ConnectionId = ConnectionId;
            await _hubConnection.SendAsync("SendUserUpdate", userUpdate);
        }
    }

    internal async Task SendChallengeAnswer(string opponentId, bool answer)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendChallengeAnswer", opponentId, answer);
        }
    }

    internal async Task SendFinishedSetup(string opponentId)
    {
        if(_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendFinishedSetup", ConnectionId, opponentId);
        }
    }

    internal async Task SendWhoStarts()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendWhoStarts", ConnectionId);
        }
    }
    #endregion
}
