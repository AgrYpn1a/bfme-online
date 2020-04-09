using BfmeOnline.Launcher.Source;
using BfmeOnline.Launcher.Source.WS;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BfmeOnline.Launcher.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window, INotifyPropertyChanged
    {
        private App _bfmeApp;

        public event PropertyChangedEventHandler PropertyChanged;
        public string Path;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _onlinePlayers;
        public int OnlinePlayers
        {
            get { return _onlinePlayers; }
            private set
            {
                _onlinePlayers = value;
                OnPropertyChanged(nameof(OnlinePlayers));
            }
        }

        public string Version
        {
            get { return Util.GetLocalVersion().ToString(); }
            private set { }
        }

        public Main()
        {
            InitializeComponent();

            _bfmeApp = (App)Application.Current;

            // Bind data
            DataContext = this;

            // Bind events
            _bfmeApp.OnOnlinePlayersChanged += BfmeApp_OnOnlinePlayersChanged;
            //_bfmeApp.OnQueued += BfmeApp_OnQueued;
            //_bfmeApp.OnMatchFound += BfmeApp_OnMatchFound;
            tbPath.Text = System.Reflection.Assembly.GetExecutingAssembly().Location;

#if DEBUG_INSTALLED
            installPanel.Visibility = Visibility.Hidden;
            downloadPanel.Visibility = Visibility.Hidden;
            playPanel.Visibility = Visibility.Visible;       
#elif DEBUG_NOTINSTALLED
            installPanel.Visibility = Visibility.Visible;
            downloadPanel.Visibility = Visibility.Hidden;
            playPanel.Visibility = Visibility.Hidden;
#elif DEBUG_INSTALLING
            installPanel.Visibility = Visibility.Hidden;
            downloadPanel.Visibility = Visibility.Visible;
            playPanel.Visibility = Visibility.Hidden;
#endif
        }

        ~Main()
        {
            // Unbind
            _bfmeApp.OnOnlinePlayersChanged -= BfmeApp_OnOnlinePlayersChanged;
        }

        private void BfmeApp_OnOnlinePlayersChanged(int value) => OnlinePlayers = value;

        private void Btn_FindMatch_Click(object sender, RoutedEventArgs e)
        {
            WS.Instance.SendMessage(new WSMessage() { EventType = WSEvent.CMD, Message = "queue" });
        }

        //private void BfmeApp_OnQueued()
        //{
        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        QMWindowSearching.Visibility = Visibility.Visible;
        //        QMWindowDefaultContent.Visibility = Visibility.Collapsed;
        //    });
        //}

        //private void BfmeApp_OnMatchFound()
        //{
        //    Application.Current.Dispatcher.Invoke(async () =>
        //    {
        //        QMWindowSearching.Visibility = Visibility.Collapsed;
        //        QMWindowFoundMatchContent.Visibility = Visibility.Visible;

        //        await Task.Delay(3000);

        //        QMWindowFoundMatchContent.Visibility = Visibility.Collapsed;
        //        QMCurrent.Visibility = Visibility.Visible;
        //    });
        //}

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.ShowDialog();
            tbPath.Text = dlg.SelectedPath;
        }
    }
}
