using System.Windows;
using System.Windows.Input;

namespace Battleships
{
    public class DragElement : BaseAttachedProperty<DragElement, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;

            if (element != null)
            {
                element.MouseMove -= Element_MouseMove;
                element.MouseMove += Element_MouseMove;
            }

        }

        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var rect = sender as FrameworkElement;

                DragDrop.DoDragDrop(rect, new DataObject((ShipViewModel)rect.DataContext), DragDropEffects.Move);
            }
        }
    }
}
