using BattleshipServer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;

namespace Battleships
{
    public partial class BattleViewModel : ObservableObject
    {
        #region Fields

        #endregion

        #region Properties

        [ObservableProperty]
        private ShipGridViewModel myShipGrid;

        [ObservableProperty]
        private ShipGridViewModel enemyShipGrid;

        [ObservableProperty]
        private HitMarkerViewModel myHitGrid = new HitMarkerViewModel();

        [ObservableProperty]
        private HitMarkerViewModel enemyHitGrid = new HitMarkerViewModel();

        #endregion

        #region Constructor

        public BattleViewModel()
        {
        }

        #endregion

        #region Server Actions


        #endregion

        #region Command method


        #endregion

        #region Methods


        #endregion
    }
}
