using BattleshipServer.Core;
using System.Text.Json;
using System;

namespace SignalRServer;

public static class Server
{
    public static List<User> Users { get; set; } = new List<User>();
    private static Random _random = new Random();

    public static void AddUser(User user)
    {
        var found = Users.FirstOrDefault(item => item.ConnectionId == user.ConnectionId);
        if(found == null)
        {
            Users.Add(user);
        }
    }

    public static User FindUser(string connectionId) => Users.FirstOrDefault(item => item.ConnectionId == connectionId);

    internal static void FinishedSetup(string senderId, string opponentId)
    {
        var finishedPlayer = FindUser(senderId);
        finishedPlayer.HasFinishedSetup = true;
        var otherPlayer = FindUser(opponentId);

        Console.WriteLine($"[{DateTime.Now}]: {finishedPlayer.Name} has finished ship setup");
        if (!otherPlayer.HasFinishedSetup)
        {
            Console.WriteLine($"[{DateTime.Now}]: Waiting for {otherPlayer.Name}");
        }

        if (finishedPlayer.HasFinishedSetup &&
            otherPlayer.HasFinishedSetup)
        {
            Console.WriteLine($"[{DateTime.Now}]: Both players, {finishedPlayer.Name} vs. {otherPlayer.Name}, have finished ship placement");

            var player = _random.Next(0, 2);
            if (player == 0)
            {
                finishedPlayer.Starts = true;
                otherPlayer.Starts = false;

                Console.WriteLine($"[{DateTime.Now}]: {finishedPlayer.Name} starts the game");
            }
            else
            {
                finishedPlayer.Starts = false;
                otherPlayer.Starts = true;

                Console.WriteLine($"[{DateTime.Now}]: {otherPlayer.Name} starts the game");
            }
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

    internal static bool WhoStarts(string senderId)
    {
        var user = FindUser(senderId);
        return user.Starts;
    }

    internal static void RemoveUser(string connectionId)
    {
        var found = Users.FirstOrDefault(item => item.ConnectionId == connectionId);
        if(found != null)
        {
            Users.Remove(found);
        }
    }
}
