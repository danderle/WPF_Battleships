using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Battleships
{
    public class AnimationPage : Page
    {
        public bool IsOldPage { get; set; }
        public bool IsFirstPage { get; set; }

        public AnimationPage()
        {
            Loaded += AnimationPge_Loaded;
        }

        private async void AnimationPge_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsOldPage)
            {
                await AnimateOut();
            }
            else
            {
                if (IsFirstPage)
                {
                    IsFirstPage = false;
                }
                else
                {
                    await AnimateIn();
                }
            }
        }

        private async Task AnimateOut()
        {
            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                From = 1,
                To = 0,
            };

            RenderTransformOrigin = new Point(0.5, 0.5);

            var scale = new ScaleTransform
            {
                CenterX = 0.5,
                CenterY = 0.5,
            };

            RenderTransform = scale;

            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(animation);
            storyboard.Begin(this);

            await Task.Delay(1000);
        }

        private async Task AnimateIn()
        {
            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                BeginTime = TimeSpan.FromSeconds(0.5),
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                From = 0,
                To = 1,
            };

            RenderTransformOrigin = new Point(0.5, 0.5);

            var scale = new ScaleTransform
            {
                CenterX = 0.5,
                CenterY = 0.5,
                ScaleY = 0
            };

            RenderTransform = scale;

            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(animation);
            storyboard.Begin(this);

            await Task.Delay(1000);
        }
    }
}
