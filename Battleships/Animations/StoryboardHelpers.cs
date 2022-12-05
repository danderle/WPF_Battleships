using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace Battleships
{
    public static class StoryboardHelpers
    {
        public static void AddScaleYFromCenter(this Storyboard storyboard, float seconds, bool timeOfset = false)
        {
            var animation = new DoubleAnimation
            {
                BeginTime = TimeSpan.FromSeconds(timeOfset ? seconds : 0),
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1,
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(animation);
        }

        public static void AddScaleYToCenter(this Storyboard storyboard, float seconds)
        {
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0,
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(animation);
        }
    }
}
