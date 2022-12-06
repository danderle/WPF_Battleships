using BattleshipServer.Core;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Battleships
{
    public class DragOverElement : BaseAttachedProperty<DragOverElement, ICommand>
    {
        private ICommand checkIfOverlappingCommand;

        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            checkIfOverlappingCommand = (ICommand)e.NewValue;
            var element = d as FrameworkElement;

            if (element != null)
            {
                element.DragOver -= Element_DragOver;
                element.DragOver += Element_DragOver;
            }

        }

        private void Element_DragOver(object sender, DragEventArgs e)
        {
            var ship = (ShipViewModel)e.Data.GetData(typeof(ShipViewModel));

            var control = sender as ItemsControl;
            Point point = e.GetPosition(control);

            double xSnap = point.X % 40;
            double ySnap = point.Y % 40;
            xSnap = point.X - xSnap;
            ySnap = point.Y - ySnap;

            if (ship.Alignment == ShipAlignment.Horizontal)
            {
                if (xSnap >= 0 && xSnap <= 400)
                {
                    if (xSnap + ship.Width >= 400)
                    {
                        xSnap -= (xSnap + ship.Width - 400);
                    }
                }

                if (ySnap >= 0 && ySnap <= 400)
                {
                    if (ySnap + ship.Height >= 400)
                    {
                        ySnap -= (ySnap + ship.Height - 400);
                    }
                }
            }
            else
            {
                if (xSnap >= 0 && xSnap <= 400)
                {
                    if (xSnap + ship.Height >= 400)
                    {
                        xSnap -= (xSnap + ship.Height - 400);
                    }
                }

                if (ySnap >= 0 && ySnap <= 400)
                {
                    if (ySnap + ship.Width >= 400)
                    {
                        ySnap -= (ySnap + ship.Width - 400);
                    }
                }
            }

            var testShip = new ShipViewModel(ship);
            testShip.Xpos = xSnap;
            testShip.Ypos = ySnap;
            testShip.SetHitCoordinates();
            checkIfOverlappingCommand.Execute(testShip);
        }
    }
}
