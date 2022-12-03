using System.Text;

namespace BattleshipServer;

public class PacketBuilder
{
    private MemoryStream _buffer;

	/// <summary>
	/// Constructor creates the memorystream
	/// </summary>
	public PacketBuilder()
	{
		_buffer = new MemoryStream();
	}

	/// <summary>
	/// Ahead of every message operation to let us know what type of data is incomming
	/// </summary>
	/// <param name="opCode"></param>
	public void WriteOpCode(byte opCode)
	{
		_buffer.WriteByte(opCode);
	}

	/// <summary>
	/// Writes the messsage to the buffer
	/// </summary>
	/// <param name="msg"></param>
	public void WriteMessage(string msg)
	{
		var msgLength = msg.Length;
		_buffer.Write(BitConverter.GetBytes(msgLength));
		_buffer.Write(Encoding.ASCII.GetBytes(msg));
	}


	/// <summary>
	/// gets the message as a byte array
	/// </summary>
	/// <returns></returns>
	public byte[] GetPacktBytes()
	{
		return _buffer.ToArray();
	}
}
