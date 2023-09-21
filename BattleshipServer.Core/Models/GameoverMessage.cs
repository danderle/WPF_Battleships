namespace BattleshipServer.Core;

public class GameOverMessage
{
    public string Winner { get; set; }
    public string Loser { get; set; }
    
    public GameOverMessage()
    {
    }

    public GameOverMessage(string winner, string loser)
    {
        Winner = winner;
        Loser = loser;
    }
}
