using BfmeOnline.Launcher.Source.commands.window.main;
using BfmeOnline.Launcher.Source.core;
using BfmeOnline.Launcher.Source.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _installPath;
        public string InstallPath
        {
            get => _installPath;
            set
            {
                _installPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InstallPath)));
            }
        }

        private int _onlinePlayers;
        public int OnlinePlayers
        {
            get => _onlinePlayers;
            set
            {
                _onlinePlayers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlinePlayers)));
            }
        }

        private Visibility _showHome = Visibility.Visible;
        public Visibility ShowHome
        {
            get => _showHome;
            private set
            {
                _showHome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowHome)));
            }
        }

        private Visibility _showInstall = Visibility.Collapsed;
        public Visibility ShowInstall
        {
            get => _showInstall; private set
            {
                _showInstall = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowInstall)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        /** Comamnds */
        public ICommand InstallGameCmd { get; private set; }
        public ICommand SelectGameCmd { get; private set; }

        public MainViewModel()
        {
            OnlinePlayers = 0;
            InstallPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            InstallGameCmd = new InstallGameCommand();
            SelectGameCmd = new SelectGameCommand();

            // Subscribe to events
            Core.Instance.OnLauncherStateChange += HandleStateChange;
        }

        ~MainViewModel()
        {
            // Unsubscribe from events
            Core.Instance.OnLauncherStateChange -= HandleStateChange;
        }

        private void HandleStateChange(LauncherState newState)
        {
            logger.Logger.LogMessage("State change");
            switch (newState)
            {
                case LauncherState.Installing:
                    {
                        logger.Logger.LogMessage($"State change {newState.ToString()}");

                        ShowInstall = Visibility.Visible;
                        ShowHome = Visibility.Collapsed;
                        break;
                    }
            }
        }
    }
}
