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
    public Action<List<User>> ReceiveUserListUpdateAction { get; internal set; }
    public Action<string> ReceiveFinishedSetupAction { get; internal set; }
    public Action<bool> ReceiveWhoStartsAction { get; internal set; }
    public Action<User> ReceiveGameoverAction { get; internal set; }
    public Action<ShotFiredMessage> ReceiveShotFiredAction { get; internal set; }
    public Action<ShotFiredMessage> ReceiveShotConfirmationAction { get; internal set; }
    public Action<ShipDestroyedMessage> ReceiveShipDestroyedAction { get; internal set; }

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
        await SendUserListUpdate();
    }

    private void DefineHubReceiveMethods()
    {
        _hubConnection.On<User>("ReceiveNewUser", user => ReceiveNewUserAction?.Invoke(user));
        _hubConnection.On<List<User>>("ReceiveUserListUpdate", list => ReceiveUserListUpdateAction?.Invoke(list));
        _hubConnection.On<string>("ReceiveChallenge", connectionId => ReceiveChallengeAction?.Invoke(connectionId));
        _hubConnection.On<User>("ReceiveUserUpdate", user => ReceiveUserUpdateAction?.Invoke(user));
        _hubConnection.On<bool>("ReceiveChallengeAnswer", answer => ReceiveChallengeAnswerAction?.Invoke(answer));
        _hubConnection.On<string>("ReceiveFinishedSetup", answer => ReceiveFinishedSetupAction?.Invoke(answer));
        _hubConnection.On<bool>("ReceiveWhoStarts", answer => ReceiveWhoStartsAction?.Invoke(answer));
        _hubConnection.On<ShotFiredMessage>("ReceiveShotFiredAction", answer => ReceiveShotFiredAction?.Invoke(answer));
        _hubConnection.On<ShotFiredMessage>("ReceiveShotConfirmation", answer => ReceiveShotConfirmationAction?.Invoke(answer));
        _hubConnection.On<ShipDestroyedMessage>("ReceiveShipDestroyed", answer => ReceiveShipDestroyedAction?.Invoke(answer));
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

    internal async Task SendUserListUpdate()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendUserListUpdate", ConnectionId);
        }
    }

    internal async Task SendShotConfirmation(ShotFiredMessage shot)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendShotConfirmation", shot);
        }
    }

    internal async Task SendShotFired(ShotFiredMessage shot)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendShotFired", shot);
        }
    }

    internal async Task SendShipDestroyed(ShipDestroyedMessage message)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SendShipDestroyed", message);
        }
    }
    #endregion
}
