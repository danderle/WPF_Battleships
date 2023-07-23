using BattleshipServer.Core;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;

namespace BattleshipServer;

internal class Client
{
    public TcpClient TcpSocket;

    private PacketReader _packetReader;

    public static string Default = "default";
    public User User { get; set; } = new User();
    public Client(TcpClient tcpClient)
    {
        TcpSocket = tcpClient;
        _packetReader = new PacketReader(TcpSocket.GetStream());

        User.ConnectionId = Guid.NewGuid().ToString();
        User.Name = Default;

        var opCode = _packetReader.ReadByte();
        var message = _packetReader.ReadMessage();

        Console.WriteLine($"[{DateTime.Now}]: A new default client is attempting to connecto to server");

        Task.Run(() => Process());
    }

    private void Process()
    {
        while (true)
        {
            try
            {
                var opCode = (OpCodes)_packetReader.ReadByte();
                string message = _packetReader.ReadMessage();

                switch (opCode)
                {
                    case OpCodes.NewUser:
                        User.Name = message;
                        Console.WriteLine($"[{DateTime.Now}]: Created a new username {User.Name}");
                        Server.BroadCastConnection();
                        break;
                    case OpCodes.ChallengePlayer:
                        User.IsBusy = true;
                        Server.BroadCastChallenge(message);
                        break;
                    case OpCodes.ChallengeAnswer:
                        Server.BroadCastChallengeAnswer(message);
                        break;
                    case OpCodes.Busy:
                        var sender = JsonSerializer.Deserialize<User>(message);
                        User.IsBusy = sender.IsBusy;
                        Server.BroadCastBusy();
                        break;
                    case OpCodes.FinishedSetup:
                        Server.BroadCastFinishedSetup(message);
                        break;
                    case OpCodes.ShotFired:
                        Server.BroadCastShotFired(message);
                        break;
                    case OpCodes.ShotConfirmation:
                        Server.BroadCastShotConfirmation(message);
                        break;
                    case OpCodes.ShipDestroyed:
                        Server.BroadCastShipDestroyed(message);
                        break;
                    case OpCodes.GameOver:
                        Server.BroadCastGameOver(message);
                        break;
                    case OpCodes.WhoStarts:
                        Server.BroadCastWhoStarts(message);
                        break;
                    case OpCodes.UpdateUserList:
                        Server.BroadCastUserList(message);
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TcpSocket.Close();
                Server.BroadCastDisconnection(User.Name);
                break;
            }
        }
    }
}
