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
}
