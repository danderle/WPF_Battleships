namespace BattleshipServer.Core;

public class ChallengeMessage
{
    public string Challenger { get; set; }

    public string Defender { get; set; }

    public ChallengeMessage()
    {
    }

    public ChallengeMessage(string challenger, string defender)
    {
        Challenger = challenger;
        Defender = defender;
    }
}
