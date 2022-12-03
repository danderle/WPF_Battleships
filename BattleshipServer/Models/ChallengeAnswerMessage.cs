namespace BattleshipServer;

public class ChallengeAnswerMessage
{
    #region Properties

    public string Challenger { get; set; }

    public string Defender { get; set; }

    public bool Accept { get; set; }

    #endregion

    #region Constructor

    public ChallengeAnswerMessage()
    {
    }

    public ChallengeAnswerMessage(string challenger, string defender, bool accept)
    {
        Challenger = challenger;
        Defender = defender;
        Accept = accept;
    } 

    #endregion
}
