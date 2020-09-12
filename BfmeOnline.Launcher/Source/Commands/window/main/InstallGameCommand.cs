using BfmeOnline.GameInstaller;
using BfmeOnline.Launcher.Source.core;
using BfmeOnline.Launcher.Source.Http;
using BfmeOnline.Launcher.Source.logger;
using BfmeOnline.Launcher.Source.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BfmeOnline.Launcher.Source.commands.window.main
{
    class InstallGameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            MainModel model = (MainModel)parameter;

            //Logger.LogMessage("[CMD] InstallGame");

            Core.Instance.ChangeState(LauncherState.Installing);

            MediaPlayer mp = new MediaPlayer();
            mp.Open(new Uri(@"Resources/Sound/install.mp3", UriKind.Relative));
            mp.Play();

            // Track progress
            new Thread(() =>
            {
                // Do a background
                Thread.CurrentThread.IsBackground = true;

                while (Installer.State != InstallerState.FINISHED && Installer.State != InstallerState.CANCELLED)
                {
                    Thread.Sleep(500);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        model.InstallProgress = Installer.Progress;

                        switch (Installer.State)
                        {
                            case InstallerState.DOWNLOADING:
                                {
                                    model.InstallState = "Downloading game files...";
                                    break;
                                }
                            case InstallerState.EXTRACTING:
                                {
                                    model.InstallState = "Installing game...";
                                    break;
                                }
                        }

                        //lbInstaller.Content = Installer.State;
                        //pbInstaller.Value = Installer.Progress;
                    });
                }

                // Handle cancellation
                if (Installer.State == InstallerState.CANCELLED)
                {
                    MessageBox.Show("Installation cancelled.");
                    Core.Instance.ChangeState(LauncherState.GameNotInstalled);
                }

                //FinalizeInstall();
            }).Start();

            // Download game files
            await Task.Run(() =>
            {
                try
                {
                    Installer.Download("https://bfme-games.fra1.digitaloceanspaces.com/The%20Battle%20for%20Middle-earth.zip");
                }
                catch (Exception e)
                {
                    Core.Instance.ChangeState(LauncherState.GameNotInstalled);
                    MessageBox.Show("Download failed!");
                }
                //Installer.Install(NetworkAddresses.BFME_DOWNLOAD, model.InstallPath);
            });

            // Check if finished because of cancellation
            if(Installer.State == InstallerState.CANCELLED) return;

            // Extract game files
            await Task.Run(async () =>
            {
                Installer.Install(model.InstallPath);
            });

            // Finalize installation
            Installer.InstallRegistryKeys(model.InstallPath);

            MessageBox.Show("Game installed!");
            Core.Instance.ChangeState(LauncherState.Game);

            // Stop music
            mp.Stop();
            mp.Close();
        }
    }
}
