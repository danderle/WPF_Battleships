using BattleshipServer.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Data;

namespace Battleships;

public partial class ShipGridViewModel : ObservableObject
{
    #region MyRegion

    private object _lock = new object();

    #endregion

    #region Properties

    public ObservableCollection<ShipViewModel> Ships { get; set; } = new ObservableCollection<ShipViewModel>();

    #endregion

    #region Constructor

    public ShipGridViewModel()
	{
		Inject.Application.Server.ShipDestroyedAction = ShipDestroyed;
		Inject.Application.SignalR.ReceiveShipDestroyedAction = ReceiveShipDestroyed;
        BindingOperations.EnableCollectionSynchronization(Ships, _lock);
    }

    #endregion

    #region Server actions

    private void ShipDestroyed(string message)
    {
        var ship = JsonSerializer.Deserialize<ShipDestroyedMessage>(message);
        
        lock (_lock)
        {
            Ships.Add(new ShipViewModel(ship));
        }
    }

    #endregion

    #region SignalR actions

    private void ReceiveShipDestroyed(ShipDestroyedMessage ship)
    {
        lock (_lock)
        {
            Ships.Add(new ShipViewModel(ship));
        }
    }

    #endregion

    #region Command methods

    [RelayCommand]
	public void CheckIfOverlapping(ShipViewModel testShip)
	{
		if (!IsOverlapping(testShip))
		{
			var setShip = Ships.FirstOrDefault(item => item.ShipType == testShip.ShipType);
			setShip.Xpos = testShip.Xpos;
			setShip.Ypos = testShip.Ypos;
			setShip.HitCoordinates = testShip.HitCoordinates;
        }
	}

    [RelayCommand]
    public void Rotate(ShipViewModel ship)
    {
        ship.Rotate();
        if (IsOverlapping(ship))
        {
            ship.Reset();
        }
    }

    #endregion

    #region Methods

    private bool IsOverlapping(ShipViewModel testShip)
    {
        foreach (var ship in Ships)
        {
            if (ship.ShipType != testShip.ShipType)
            {
                foreach (var coo in ship.HitCoordinates)
                {
                    foreach (var testCoo in testShip.HitCoordinates)
                    {
                        if (coo.Compare(testCoo))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    #endregion
}
