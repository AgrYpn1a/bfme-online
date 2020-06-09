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
using BfmeOnline.Downloader;
using System.Threading;
using BfmeOnline.GameInstaller;
using BfmeOnline.Launcher.Source.Http;
using System.Diagnostics;
using BfmeOnline.Launcher.Source.Updates;
using System.Security.Cryptography;
using System.IO;
using BfmeOnline.Launcher.Source.ViewModel;

namespace BfmeOnline.Launcher.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private App _bfmeApp;

        private MainViewModel _viewModel;

        #region Constructor
        public Main()
        {
            InitializeComponent();

            _bfmeApp = (App)Application.Current;

            // Setup view model and data context
            _viewModel = new MainViewModel();
            DataContext = _viewModel;


            // Bind data

            //_bfmeApp.OnQueued += BfmeApp_OnQueued;
            //_bfmeApp.OnMatchFound += BfmeApp_OnMatchFound;

            //if (RegistryManager.IsGameInstalled())
            //{
            //    installPanel.Visibility = Visibility.Hidden;
            //    downloadPanel.Visibility = Visibility.Hidden;
            //    playPanel.Visibility = Visibility.Visible;
            //}

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
            installPanel.Visibility = Visibility.Hidden;
            downloadPanel.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                //Installer.Install("https://bfme-games.fra1.digitaloceanspaces.com/The%20Battle%20for%20Middle-earth.zip", Path);
                Installer.Install(NetworkAddresses.BFME_DOWNLOAD, _viewModel.InstallPath);
            });
            Task.Run(() =>
            {
                while (Installer.State != InstallerState.FINISHED)
                {
                    Thread.Sleep(500);

                    Dispatcher.Invoke(() =>
                    {
                        lbInstaller.Content = Installer.State;
                        pbInstaller.Value = Installer.Progress;
                    });

                }
                FinalizeInstall();
            });
        }

        private void FinalizeInstall()
        {
            MessageBox.Show("Installation finnished!");
            //playPanel.Visibility = Visibility.Hidden;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.ShowDialog();
            tbPath.Text = dlg.SelectedPath;
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
