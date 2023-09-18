namespace BattleshipServer.Core;

public class ShotFiredMessage
{
    public bool Hit { get; set; }
    public string Opponent { get; set; }
    public double Xpos { get; set; }
    public double Ypos { get; set; }
    public string OpponentId { get; set; }
    public string AttackerId { get; set; }

    public ShotFiredMessage()
    {
    }

    public ShotFiredMessage(string opponent, double xPos, double yPos)
    {
        Opponent = opponent;
        Xpos = xPos;
        Ypos = yPos;
    }

    public ShotFiredMessage(double xPos, double yPos, string opponentId, string attackerId)
    {
        OpponentId = opponentId;
        AttackerId = attackerId;
        Xpos = xPos;
        Ypos = yPos;
    }
}
