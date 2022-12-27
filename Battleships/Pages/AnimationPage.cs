using System.Threading.Tasks;
using System.Windows.Controls;

namespace Battleships
{
    public class AnimationPage : Page
    {
        private const float _animationSeconds = 0.3f;
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
            await this.ScaleYToCenter(_animationSeconds);
        }

        private async Task AnimateIn()
        {
            await this.ScaleYFromCenter(_animationSeconds);
        }
    }
}
