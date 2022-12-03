namespace BattleshipServer
{
    public class User
    {
        public bool IsConnected { get; set; }
        public bool IsBusy { get; set; }
        public string Name { get; set; }
        public bool HasFinishedSetup { get; set; }
    }
}
