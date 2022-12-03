using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;

namespace BattleshipServer;

internal class Client
{
    public TcpClient TcpSockect;

    private PacketReader _packetReader;

    public User User { get; set; } = new User();
    public Client(TcpClient tcpClient)
    {
        TcpSockect = tcpClient;
        _packetReader = new PacketReader(TcpSockect.GetStream());

        var opCode = _packetReader.ReadByte();
        User.Name = _packetReader.ReadMessage();

        Console.WriteLine($"New client {User.Name}, attempting to connect");

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
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }
    }
}
