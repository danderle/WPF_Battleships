﻿using BattleshipServer.Core;
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

    public Action<string> NewUserAction;
    public Action<string> ChallengePlayerAction;
    public Action<string> ChallengeAnswerAction;
    public Action<string> BusyAction;
    public Action<string> FinishedSetupAction;
    public Action<string> ShotFiredAction;
    public Action<string> ShotConfirmationAction;
    public Action<string> ShipDestroyedAction;
    public Action<string> GameoverAction;
    public Action<string> WhoStartsAction;
    public Action<string> DisconnectedClientAction;
    public Action<string> UpdateUserListAction;

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

            CreateAndSendPacket(OpCodes.DefaultClient, "default");

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
                    case OpCodes.NewUser:
                        NewUserAction?.Invoke(message);
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
                    case OpCodes.ShotConfirmation:
                        ShotConfirmationAction?.Invoke(message);
                        break;
                    case OpCodes.ShipDestroyed:
                        ShipDestroyedAction?.Invoke(message);
                        break;
                    case OpCodes.GameOver:
                        GameoverAction?.Invoke(message);
                        break;
                    case OpCodes.WhoStarts:
                        WhoStartsAction?.Invoke(message);
                        break;
                    case OpCodes.DisconnectedClient:
                        DisconnectedClientAction?.Invoke(message);
                        break;
                    case OpCodes.UpdateUserList:
                        UpdateUserListAction?.Invoke(message);
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
