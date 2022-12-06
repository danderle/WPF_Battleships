namespace BattleshipServer.Core;

public class ShotFiredMessage
{
    public bool Hit { get; set; }
    public string Opponent { get; set; }
    public double Xpos { get; set; }
    public double Ypos { get; set; }

    public ShotFiredMessage()
    {
    }

    public ShotFiredMessage(string opponent, double xPos, double yPos)
    {
        Opponent = opponent;
        Xpos = xPos;
        Ypos = yPos;
    }
}
