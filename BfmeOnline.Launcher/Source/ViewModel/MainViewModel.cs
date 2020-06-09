using BfmeOnline.Launcher.Source.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        public string Version
        {
            get { return Util.GetLocalVersion().ToString(); }
            private set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            OnlinePlayers = 0;
            InstallPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        }
    }
}
