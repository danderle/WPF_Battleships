namespace BattleshipServer;

public enum OpCodes
{
    Connect,
    ChallengePlayer,
    ChallengeAnswer,
    Busy,
    FinishedSetup,
    ShotFired,
    ShotConfirmation,
    ShipDestroyed,
    GameOver,
    WhoStarts,
}
