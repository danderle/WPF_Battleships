using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Battleships
{
    public class ClickElement : BaseAttachedProperty<ClickElement, ICommand>
    {
        private ICommand fireCommand;
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            fireCommand = (ICommand)e.NewValue;
            var element = d as FrameworkElement;

            if (element != null && fireCommand != null)
            {
                element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
                element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
            }

        }

        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            Point point = e.GetPosition(canvas);

            double xSnap = point.X % 40;
            double ySnap = point.Y % 40;
            xSnap = point.X - xSnap;
            ySnap = point.Y - ySnap;

            var shot = new ShotFiredViewModel(xSnap, ySnap);
            fireCommand.Execute(shot);
        }
    }
}
