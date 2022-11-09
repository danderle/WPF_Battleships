using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public partial class ApplicationViewModel : ObservableObject
    {
        [ObservableProperty]
        private ApplicationPages currentPage = ApplicationPages.MainMenuPage;

        public ApplicationViewModel()
        {

        }
    }
}
