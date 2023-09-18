namespace BattleshipServer.Core;

public class ShipDestroyedMessage
{
    public string Opponent { get; set; }
    public string OpponentId { get; set; }
    public string AttackerId { get; set; }
    public ShipTypes ShipType { get; set; }
    public ShipAlignment Alignment { get; set; }
    public double Xpos { get; set; }
    public double Ypos { get; set; }

    public ShipDestroyedMessage()
    {

    }

    public ShipDestroyedMessage(string opponentName, ShipTypes shipType, ShipAlignment alignment, double xpos, double ypos)
    {
        Opponent = opponentName;
        ShipType = shipType;
        Alignment = alignment;
        Xpos = xpos;
        Ypos = ypos;
    }

    public ShipDestroyedMessage(ShipTypes shipType, ShipAlignment alignment, double xpos, double ypos, string opponentId, string attackerId)
    {
        OpponentId = opponentId;
        AttackerId = attackerId;
        ShipType = shipType;
        Alignment = alignment;
        Xpos = xpos;
        Ypos = ypos;
    }
}
