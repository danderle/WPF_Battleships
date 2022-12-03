using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Battleships
{
    public class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as Frame;

            if (frame != null)
            {
                //Hide the navigation bar
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

                //clear page history
                frame.Navigated += (s, e) => ((Frame)s).NavigationService.RemoveBackEntry();
            }

        }

    }
}
