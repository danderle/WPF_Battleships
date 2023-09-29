using BattleshipServer.Core;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer;

public class BattleshipsHub : Hub
{
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Server.RemoveUser(Context.ConnectionId);
        await Clients.All.SendAsync("ReceiveUserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendNewUser(User user)
    {
        Server.AddUser(user);
        await Clients.All.SendAsync("ReceiveNewUser", user);
    }

    public async Task SendUserListUpdate(string connectionId)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveUserListUpdate", Server.Users);
    }

    public async Task SendChallenge(string opponentId, string challengerId)
    {
        await Clients.Client(opponentId).SendAsync("ReceiveChallenge", challengerId);
    }

    public async Task SendUserUpdate(User user)
    {
        Server.Update(user);
        await Clients.All.SendAsync("ReceiveUserUpdate", user);
    }

    public async Task SendChallengeAnswer(string opponentId, bool answer)
    {
        await Clients.Client(opponentId).SendAsync("ReceiveChallengeAnswer", answer);
    }

    public async Task SendFinishedSetup(string senderId, string opponentId)
    {
        Server.FinishedSetup(senderId, opponentId);
        await Clients.Client(opponentId).SendAsync("ReceiveFinishedSetup", senderId);
    }

    public async Task SendWhoStarts(string senderId)
    {
        await Clients.Client(senderId).SendAsync("ReceiveWhoStarts", Server.WhoStarts(senderId));
    }

    public async Task SendShotConfirmation(ShotFiredMessage shot)
    {
        await Clients.Client(shot.AttackerId).SendAsync("ReceiveShotConfirmation", shot);
    }

    public async Task SendShotFired(ShotFiredMessage shot)
    {
        await Clients.Client(shot.OpponentId).SendAsync("ReceiveShotFired", shot);
    }

    public async Task SendShipDestroyed(ShipDestroyedMessage message)
    {
        await Clients.Client(message.AttackerId).SendAsync("ReceiveShipDestroyed", message);
    }

    public async Task SendGameoverMessage(GameOverMessage message)
    {
        await Clients.Client(message.Winner).SendAsync("ReceiveGameoverMessage", message);
        await Clients.Client(message.Loser).SendAsync("ReceiveGameoverMessage", message);
    }
}
