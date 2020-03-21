using System.ComponentModel;
using System.Windows;

namespace BfmeOnline.Launcher.View
{
    /// <summary>
    /// Interaction logic for Updater.xaml
    /// </summary>
    public partial class Updater : Window, INotifyPropertyChanged
    {

        public Updater()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _downloadProgress = 0;
        public int DownloadProgress
        {
            get { return _downloadProgress; }
            private set
            {
                _downloadProgress = value;
                OnPropertyChanged(nameof(DownloadProgress));
            }
        }

        private string _progressMessage;
        public string ProgressMessage
        {
            get { return _progressMessage; }
            private set
            {
                _progressMessage = value;
                OnPropertyChanged(nameof(ProgressMessage));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetDownloadProgress(int currentProgress)
        {
            this.DownloadProgress = currentProgress;
        }

        public void SetMessage(string message)
        {
            this.ProgressMessage = message;
        }
    }
}
