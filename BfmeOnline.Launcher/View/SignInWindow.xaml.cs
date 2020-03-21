using BfmeOnline.Launcher.Source.Auth;
using BfmeOnline.Launcher.Source.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window, INotifyPropertyChanged
    {
        public string Email { get; set; } = "tojagic.rastko@gmail.com";

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        #region Property Binding
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        private App _bfmeApp;

        public SignInWindow()
        {
            InitializeComponent();

            _bfmeApp = (App)Application.Current;
            _bfmeApp.OnAuthorized += BfmeApp_OnAuthorized;

            DataContext = this;
        }

        ~SignInWindow()
        {
            _bfmeApp.OnAuthorized -= BfmeApp_OnAuthorized;
        }

        private void BfmeApp_OnAuthorized()
        {
            this.Close();
        }

        private async void Btn_SignIn_Click(object sender, RoutedEventArgs e)
        {
            // Clear error message
            Message = "";
            DisableControls();

            WebResponse response = await AuthManager.Instance.Authenticate(Email, Txt_Password.Password);
            await Task.Delay(1000);

            EnableControls();

            if (response.Status == ResponseStatus.OK)
            {
                _bfmeApp.CheckForAuth();
            }
            else
            {
                // An error has occurred
                Message = response.Message;
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.thebfmeonline.com") { UseShellExecute = true });
        }

        private void DisableControls()
        {
            // Show loading
            ButtonsPanel.Visibility = Visibility.Collapsed;
            Loading.Visibility = Visibility.Visible;

            Txt_Email.IsEnabled = false;
            Txt_Password.IsEnabled = false;
        }

        private void EnableControls()
        {
            // Hide loading
            ButtonsPanel.Visibility = Visibility.Visible;
            Loading.Visibility = Visibility.Collapsed;

            Txt_Email.IsEnabled = true;
            Txt_Password.IsEnabled = true;
        }
    }
}
