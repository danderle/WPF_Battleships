using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Battleships
{
    public static class PageAnimations
    {
        public static async Task ScaleYToCenter(this Page page, float seconds)
        {
            page.RenderTransformOrigin = new Point(0.5, 0.5);
            var scale = new ScaleTransform
            {
                CenterX = 0.5,
                CenterY = 0.5,
            };

            page.RenderTransform = scale;

            var storyboard = new Storyboard();
            storyboard.AddScaleYToCenter(seconds);
            storyboard.Begin(page);

            await Task.Delay((int)(1000*seconds));
        }

        public static async Task ScaleYFromCenter(this Page page, float seconds)
        {
            page.RenderTransformOrigin = new Point(0.5, 0.5);
            var scale = new ScaleTransform
            {
                CenterX = 0.5,
                CenterY = 0.5,
                ScaleY = 0
            };

            page.RenderTransform = scale;

            var storyboard = new Storyboard();
            storyboard.AddScaleYFromCenter(seconds, true);
            storyboard.Begin(page);

            await Task.Delay((int)(1000 * seconds));
        }
    }
}
