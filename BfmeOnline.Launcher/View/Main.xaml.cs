using BfmeOnline.Launcher.Source;
using BfmeOnline.Launcher.Source.ViewModel;
using BfmeOnline.Launcher.Source.WS;
using System;
using System.Diagnostics;
using System.Windows;

namespace BfmeOnline.Launcher.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private App m_bfmeApp;

        private MainViewModel m_viewModel;

        #region Constructor
        public Main()
        {
            InitializeComponent();

            m_bfmeApp = (App)Application.Current;
            m_viewModel = new MainViewModel(this);

#if DEBUG_INSTALLED
            installPanel.Visibility = Visibility.Hidden;
            downloadPanel.Visibility = Visibility.Hidden;
            playPanel.Visibility = Visibility.Visible;       
#elif DEBUG_NOTINSTALLED
            installPanel.Visibility = Visibility.Visible;
            downloadPanel.Visibility = Visibility.Collapsed;
            playPanel.Visibility = Visibility.Collapsed;
#elif DEBUG_INSTALLING
            installPanel.Visibility = Visibility.Hidden;
            downloadPanel.Visibility = Visibility.Visible;
            playPanel.Visibility = Visibility.Hidden;
#endif
        }

        ~Main()
        {
        }

        #endregion

        #region Lifecycle Methods

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            // Get hashsums
            //GameUpdateManager.GetHashSums(RegistryManager.GetInstallPath());
        }

        #endregion

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

        private bool _isInstalling;
        private bool _isDownloading;

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            //installPanel.Visibility = Visibility.Hidden;
            //downloadPanel.Visibility = Visibility.Visible;
            //Task.Run(() =>
            //{
            //    //Installer.Install("https://bfme-games.fra1.digitaloceanspaces.com/The%20Battle%20for%20Middle-earth.zip", Path);
            //    Installer.Install(NetworkAddresses.BFME_DOWNLOAD, _viewModel.InstallPath);
            //});
            //Task.Run(() =>
            //{
            //    while (Installer.State != InstallerState.FINISHED)
            //    {
            //        Thread.Sleep(500);

            //        Dispatcher.Invoke(() =>
            //        {
            //            lbInstaller.Content = Installer.State;
            //            pbInstaller.Value = Installer.Progress;
            //        });

            //    }
            //    FinalizeInstall();
            //});
        }

        private void FinalizeInstall()
        {
            MessageBox.Show("Installation finnished!");
            //playPanel.Visibility = Visibility.Hidden;
        }

        private void Btn_Play(object sender, RoutedEventArgs e)
        {
            try
            {
                string bfmeGameFile = $"{LauncherData.bfmeGameInstallPath}\\lotrbfme.exe";
                Process.Start(bfmeGameFile);

            }
            catch (Exception err)
            {
                MessageBox.Show("Game files not found!");
                //TODO: Registry cleanup
            }
        }
    }
}
