using BfmeOnline.Launcher.Source;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BfmeOnline.Launcher.View.Components
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public string Version
        {
            get { return Util.GetLocalVersion().ToString(); }
            private set { }
        }

        public TitleBar()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Minimize(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<Main>().SingleOrDefault(window => { window.WindowState = WindowState.Minimized; return true; });
        }

        private void Button_Maximize(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
