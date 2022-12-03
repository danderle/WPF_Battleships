using System;
using System.Windows;
using System.Windows.Controls;

namespace Battleships
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {
        #region Dependency property

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(AnimationPage), typeof(PageHost), new PropertyMetadata(CurrentPagePropertyChanged));

        #endregion

        #region Properties

        public AnimationPage CurrentPage
        {
            get => (AnimationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        #endregion

        #region Constructor

        public PageHost()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldPageFrame = (d as PageHost).OldPage;
            var newPageFrame = (d as PageHost).NewPage;

            var oldPageContent = newPageFrame.Content as AnimationPage;

            var newPage = e.NewValue as AnimationPage;
            if (oldPageContent == null)
            {
                newPage.IsFirstPage = true;
            }
            else
            {
                oldPageContent.IsOldPage = true;
                newPageFrame.Content = null;
            }

            newPageFrame.Content = newPage;
            oldPageFrame.Content = oldPageContent;
        }

        #endregion
    }
}
