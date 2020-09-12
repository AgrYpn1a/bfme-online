using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace BfmeOnline.Launcher.View.Components
{
    /// <summary>
    /// Interaction logic for ImageSlideshow.xaml
    /// </summary>
    public partial class ImageSlideshow : UserControl
    {
        // Game name base
        private string _currentGame = "bfme1";
        private int _slideIndex = 0;

        public int SlideCount = 3;

        DoubleAnimation fadeOut = new DoubleAnimation();
        DoubleAnimation fadeIn = new DoubleAnimation();

        public ImageSlideshow()
        {
            InitializeComponent();

            fadeOut.From = 1;
            fadeOut.To = 0;
            fadeOut.Duration = TimeSpan.FromSeconds(3);
            fadeOut.BeginTime = TimeSpan.FromSeconds(0);

            ImgSliderCurrent.Source = new BitmapImage(new Uri(@$"../../Resources/Images/Carousel/{_currentGame}_{_slideIndex}.jpg", UriKind.Relative));

            fadeOut.Completed += (object sender, EventArgs e) =>
            {
                ImgSliderCurrent.Source = new BitmapImage(new Uri(@$"../../Resources/Images/Carousel/{_currentGame}_{_slideIndex}.jpg", UriKind.Relative));
                _slideIndex = (_slideIndex + 1) % SlideCount;

                ImgSliderCurrent.BeginAnimation(Image.OpacityProperty, fadeIn);
            };

            fadeIn.From = 0;
            fadeIn.To = 1;
            fadeIn.Duration = TimeSpan.FromSeconds(3);
            fadeIn.BeginTime = TimeSpan.FromSeconds(0);

            fadeIn.Completed += (object sender, EventArgs e) =>
            {
                ImgSliderNext.Source = new BitmapImage(new Uri(@$"../../Resources/Images/Carousel/{_currentGame}_{_slideIndex}.jpg", UriKind.Relative));

                ImgSliderCurrent.BeginAnimation(Image.OpacityProperty, fadeOut);
            };

            ImgSliderCurrent.BeginAnimation(Image.OpacityProperty, fadeOut);
        }
    }
}
