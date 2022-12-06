using System.Net.Sockets;
using System.Text;

namespace BattleshipServer.Core;

public class PacketReader : BinaryReader
{
    private NetworkStream _stream;

    public PacketReader(NetworkStream stream) : base(stream)
    {
        _stream = stream;
    }

    /// <summary>
    /// Reads the entire message into a buffer an converts it back to string
    /// </summary>
    /// <returns></returns>
    public string ReadMessage()
    {
        byte[] buffer;
        var length = ReadInt32();
        buffer = new byte[length];
        _stream.Read(buffer, 0, length);

        var msg = Encoding.ASCII.GetString(buffer);
        return msg;
    }
}
