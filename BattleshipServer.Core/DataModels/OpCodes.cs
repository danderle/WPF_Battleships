namespace BattleshipServer.Core;

public enum OpCodes
{
    DefaultClient,
    NewUser,
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
