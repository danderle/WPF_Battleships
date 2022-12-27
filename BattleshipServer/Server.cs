using BattleshipServer.Core;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;

namespace BattleshipServer;

public class Server
{
    private static List<Client> _users = new List<Client>();

	private static Random _random = new Random();

	public Server()
	{
		Run();
	}

	private void Run()
	{
		Console.WriteLine($"[{DateTime.Now}]: Battleship Server online!");

		TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
		listener.Start();

		Console.WriteLine($"[{DateTime.Now}]: Listening for incomming connections on localhost");

		while (true)
		{
			var client = new Client(listener.AcceptTcpClient());
			_users.Add(client);

			Console.WriteLine($"[{DateTime.Now}]: A new default client has connected to the server");

			BroadCastConnection();
		}
    }

    #region Pack message and send

    private static void CreateAndSendPacket(Client user, OpCodes code, string message)
    {
        if (user == null)
        {
            return;
        }

        var packet = new PacketBuilder();
        packet.WriteOpCode((byte)code);
        packet.WriteMessage(message);

        user?.TcpSocket.Client.Send(packet.GetPacktBytes());
    }

    #endregion

    #region Broad cast methods

    public static void BroadCastConnection()
	{
		foreach (var user in _users)
		{
			foreach (var other in _users)
			{
				if (other.User.Name != Client.Default)
				{
					var message = other.User.Name;
					CreateAndSendPacket(user, OpCodes.NewUser, message);
				}
            }
		}
	}

    public static void BroadCastChallenge(string message)
    {
        ChallengeMessage msg = JsonSerializer.Deserialize<ChallengeMessage>(message);
		var defender = FindClient(msg.Defender);
		defender.User.IsBusy = true;
		CreateAndSendPacket(defender, OpCodes.ChallengePlayer, message);

		Console.WriteLine($"[{DateTime.Now}]: {msg.Challenger} has challenged {defender.User.Name}");
    }

	internal static void BroadCastChallengeAnswer(string message)
	{
        ChallengeAnswerMessage msg = JsonSerializer.Deserialize<ChallengeAnswerMessage>(message);
        var challenger = FindClient(msg.Challenger);
        var defender = FindClient(msg.Defender);
        CreateAndSendPacket(challenger, OpCodes.ChallengeAnswer, message);

		if (msg.Accept)
		{
            Console.WriteLine($"[{DateTime.Now}]: {msg.Defender} has accepted {msg.Challenger} challenge");
        }
        else
		{
			Console.WriteLine($"[{DateTime.Now}]: {msg.Defender} has denied {msg.Challenger} challenge");
			challenger.User.IsBusy = false;
			defender.User.IsBusy = false;
			BroadCastBusy();
        }
    }

    internal static void BroadCastBusy()
    {
        foreach (var user in _users)
		{
			Console.WriteLine($"[{DateTime.Now}]: {user.User.Name} is busy -- {user.User.IsBusy}");
			foreach (var other in _users)
			{
				var message = JsonSerializer.Serialize(other.User);
				CreateAndSendPacket(user, OpCodes.Busy, message);
			}
		}
    }

    internal static void BroadCastFinishedSetup(string message)
    {
        var finishedMessage = JsonSerializer.Deserialize<ChallengeMessage>(message);
		var finishedPlayer = FindClient(finishedMessage.Challenger);
		finishedPlayer.User.HasFinishedSetup = true;
		var otherPlayer = FindClient(finishedMessage.Defender);

		Console.WriteLine($"[{DateTime.Now}]: {finishedPlayer.User.Name} has finished ship setup");
		if (!otherPlayer.User.HasFinishedSetup)
		{
			Console.WriteLine($"[{DateTime.Now}]: Waiting for {otherPlayer.User.Name}");
		}

		if (finishedPlayer.User.HasFinishedSetup &&
			otherPlayer.User.HasFinishedSetup)
		{
			Console.WriteLine($"[{DateTime.Now}]: Both players, {finishedPlayer.User.Name} vs. {otherPlayer.User.Name}, have finished ship placement");

			var player = _random.Next(0, 2);
			if (player == 0)
			{
				finishedPlayer.User.Starts = true;
				otherPlayer.User.Starts = false;

                Console.WriteLine($"[{DateTime.Now}]: {finishedPlayer.User.Name} starts the game");
            }
			else
            {
                finishedPlayer.User.Starts = false;
                otherPlayer.User.Starts = true;

                Console.WriteLine($"[{DateTime.Now}]: {otherPlayer.User.Name} starts the game");
            }
			
			finishedPlayer.User.HasFinishedSetup = false;
			otherPlayer.User.HasFinishedSetup = false;
		}

		CreateAndSendPacket(otherPlayer, OpCodes.FinishedSetup, message);
    }

    internal static void BroadCastShotFired(string message)
    {
		var shotFired = JsonSerializer.Deserialize<ShotFiredMessage>(message);
		var target = FindClient(shotFired.Opponent);

		Console.WriteLine($"[{DateTime.Now}]: Shot fired at {target.User.Name}");
		CreateAndSendPacket(target, OpCodes.ShotFired, message);
    }

	internal static void BroadCastShotConfirmation(string message)
	{
        var shotFired = JsonSerializer.Deserialize<ShotFiredMessage>(message);
        var target = FindClient(shotFired.Opponent);

        Console.WriteLine($"[{DateTime.Now}]: Shot confirmation for {target.User.Name}");
		Console.WriteLine($"[{DateTime.Now}]: Shot is a " + (shotFired.Hit? "hit" : "miss"));
        CreateAndSendPacket(target, OpCodes.ShotConfirmation, message);
    }

    internal static void BroadCastShipDestroyed(string message)
    {
        var shipDestroyed = JsonSerializer.Deserialize<ShipDestroyedMessage>(message);
        var target = FindClient(shipDestroyed.Opponent);

        Console.WriteLine($"[{DateTime.Now}]: User {target.User.Name} has destroyed a {shipDestroyed.ShipType}");
        CreateAndSendPacket(target, OpCodes.ShipDestroyed, message);
    }

	internal static void BroadCastGameOver(string message)
	{
        var gameOver = JsonSerializer.Deserialize<GameOverMessage>(message);
		var winner = FindClient(gameOver.Winner);
        var loser = FindClient(gameOver.Loser);

        Console.WriteLine($"[{DateTime.Now}]: {winner.User.Name} vs {loser.User.Name } result:");
        Console.WriteLine($"[{DateTime.Now}]: {winner.User.Name} is the WINNER!");

        CreateAndSendPacket(winner, OpCodes.GameOver, message);
        CreateAndSendPacket(loser, OpCodes.GameOver, message);
    }

	internal static void BroadCastWhoStarts(string name)
	{
		var player = FindClient(name);

		Console.WriteLine($"[{DateTime.Now}]: {player.User.Name} starts the game -> {player.User.Starts}");

		CreateAndSendPacket(player, OpCodes.WhoStarts, player.User.Starts.ToString());
    }

	internal static void BroadCastDisconnection(string disconnectedUsername)
	{
		var disconnectedClient = FindClient(disconnectedUsername);
		_users.Remove(disconnectedClient);
		
		Console.WriteLine($"[{DateTime.Now}]: {disconnectedUsername} connection has been closed and removed from active user list.");

        foreach (var user in _users)
		{
			CreateAndSendPacket(user, OpCodes.DisconnectedClient, disconnectedUsername);
		}
	}

    internal static void BroadCastUserList(string username)
    {
		var list = new List<User>();
        foreach (var client in _users)
		{
			list.Add(client.User);
            Console.WriteLine($"[{DateTime.Now}]: {client.User.Name} is still connected");
        }

		var message = JsonSerializer.Serialize(list);
		var user = FindClient(username);
		CreateAndSendPacket(user, OpCodes.UpdateUserList, message);
    }

	#endregion

	#region Private Helpers

	private static Client FindClient(string name)
	{
		return _users.FirstOrDefault(item => item.User.Name == name);
    }

	#endregion
}
