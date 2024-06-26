﻿using BfmeOnline.Launcher.Source.commands.window.main;
using BfmeOnline.Launcher.Source.Commands;
using BfmeOnline.Launcher.Source.Commands.window.main;
using BfmeOnline.Launcher.Source.ViewModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.model
{
    public sealed class MainModel : AModel<MainModel, MainViewModel>
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
            set
            {
                _showHome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowHome)));
            }
        }

        private Visibility _showInstall = Visibility.Collapsed;
        public Visibility ShowInstall
        {
            get => _showInstall;
            set
            {
                _showInstall = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowInstall)));
            }
        }

        private Visibility _showUserTitleBar = Visibility.Visible;
        public Visibility ShowUserTitleBar
        {
            get => _showUserTitleBar;
            set
            {
                _showUserTitleBar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowUserTitleBar)));
            }
        }

        public Visibility _showLoading = Visibility.Collapsed;
        public Visibility ShowLoading
        {
            get => _showLoading;
            set
            {
                _showLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowLoading)));
            }
        }

        private Visibility _showPlayScreen = Visibility.Collapsed;
        public Visibility ShowPlayScreen
        {
            get => _showPlayScreen;
            set
            {
                _showPlayScreen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowPlayScreen)));
            }
        }

        public Visibility _showGameNotInstalled = Visibility.Collapsed;
        public Visibility ShowGameNotInstalled
        {
            get => _showGameNotInstalled;
            set
            {
                _showGameNotInstalled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowGameNotInstalled)));
            }
        }

        private long _progress;
        public long InstallProgress
        {
            get => _progress;
            set
            {
                _progress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InstallProgress)));
            }
        }

        private string _installerState;
        public string InstallState
        {
            get => _installerState;
            set
            {
                _installerState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InstallState)));
            }
        }

        #region PlayScreen

        private Visibility mv_checkingForUpdates = Visibility.Visible;

        public Visibility v_CheckingForUpdates
        {
            get => mv_checkingForUpdates;
            set
            {
                mv_checkingForUpdates = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(v_CheckingForUpdates)));
            }
        }

        #endregion

        /** Commands */
        public ICommand InstallGameCmd { get; private set; }
        public ICommand BackToHomeCmd { get; private set; }
        public ICommand SelectGameCmd { get; private set; }
        public ICommand BrowseInstallPathCmd { get; private set; }
        public ICommand CancelInstallCmd { get; private set; }
        public ICommand StartGameCmd { get; private set; }

        public override event PropertyChangedEventHandler PropertyChanged;

        public MainModel(MainViewModel viewModel) : base(viewModel)
        {
            InstallPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            InstallGameCmd = new InstallGameCommand();
            SelectGameCmd = new SelectGameCommand();
            BrowseInstallPathCmd = new BrowseInstallPathCommand();
            BackToHomeCmd = new BackToHomeCommand();
            CancelInstallCmd = new CancelInstallationCommand();
            StartGameCmd = new StartGameCommand();
        }

    }
}
