using CommunityToolkit.Mvvm.ComponentModel;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        public ClientToServer Server { get; private set; } = new ClientToServer();
        public string OpponentName { get; set; }

        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.MainMenuPage;

        public ApplicationViewModel()
        {

        }
    }
}
