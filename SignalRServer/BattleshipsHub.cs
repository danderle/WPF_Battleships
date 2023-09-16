using BattleshipServer.Core;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer;

public class BattleshipsHub : Hub
{
    public async Task SendNewUser(User user)
    {
        Server.AddUser(user);
        await Clients.All.SendAsync("ReceiveNewUser", user);
        await Clients.Client(user.ConnectionId).SendAsync("ReceiveUserList", Server.Users);
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
}
