namespace BattleshipServer.Core
{
    public class User
    {
        public Guid Id { get; set; }
        public bool IsConnected { get; set; }
        public bool IsBusy { get; set; }
        public string Name { get; set; }
        public bool HasFinishedSetup { get; set; }

        public bool Starts { get; set; }
    }
}
