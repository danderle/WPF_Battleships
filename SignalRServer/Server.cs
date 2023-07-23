using BattleshipServer.Core;

namespace SignalRServer;

public static class Server
{
    public static List<User> Users { get; set; } = new List<User>();

    public static void AddUser(User user)
    {
        var found = Users.FirstOrDefault(item => item.ConnectionId == user.ConnectionId);
        if(found == null)
        {
            Users.Add(user);
        }
    }

    internal static void Update(User user)
    {
        var found = Users.FirstOrDefault(item => item.ConnectionId == user.ConnectionId);
        if (found != null)
        {
            found.IsBusy = user.IsBusy;
            found.HasFinishedSetup = user.HasFinishedSetup;
        }
    }
}
