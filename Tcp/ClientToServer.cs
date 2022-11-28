using BattleshipServer;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Battleships;

public class ClientToServer
{
    private TcpClient _client;
    private PacketReader _packetReader;

    public string Username;

    public Action<string> ConnectedAction;
    public Action<string> ChallengePlayerAction;
    public Action<string> ChallengeAnswerAction;
    public Action<string> BusyAction;
    public Action<string> FinishedSetupAction;
    public Action<string> ShotFiredAction;

    public ClientToServer()
    {
        _client = new TcpClient();
    }

    public async void ConnectToServer()
    {
        if (!_client.Connected)
        {
            _client.Connect("127.0.0.1", 8080);
            _packetReader = new PacketReader(_client.GetStream());

            var packet = new PacketBuilder();
            packet.WriteOpCode((byte)OpCodes.Connect);
            packet.WriteMessage(Username);
            _client.Client.Send(packet.GetPacktBytes());

            await ReadPackets();
        }
    }

    private async Task ReadPackets()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                var opCode = (OpCodes)_packetReader.ReadByte();
                var message = _packetReader.ReadMessage();

                switch (opCode)
                {
                    case OpCodes.Connect:
                        ConnectedAction?.Invoke(message);
                        break;
                    case OpCodes.ChallengePlayer:
                        ChallengePlayerAction?.Invoke(message);
                        break;
                    case OpCodes.ChallengeAnswer:
                        ChallengeAnswerAction?.Invoke(message);
                        break;
                    case OpCodes.Busy:
                        BusyAction?.Invoke(message);
                        break;
                    case OpCodes.FinishedSetup:
                        FinishedSetupAction?.Invoke(message);
                        break;
                    case OpCodes.ShotFired:
                        ShotFiredAction?.Invoke(message);
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
        });
    }

    public void CreateAndSendPacket(OpCodes code, string message)
    {
        var packet = new PacketBuilder();
        packet.WriteOpCode((byte)code);
        packet.WriteMessage(message);

        _client.Client.Send(packet.GetPacktBytes());
    }
}
