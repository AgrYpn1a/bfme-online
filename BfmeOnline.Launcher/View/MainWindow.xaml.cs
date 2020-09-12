using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BfmeOnline.Launcher.Model;
using BfmeOnline.Launcher.Source;
using BfmeOnline.Launcher.Source.logger;
using BfmeOnline.Launcher.ViewModel;

namespace BfmeOnline.Launcher
{
    // TODO: Mora se sve prefaktorisati logika postaje skakljiva i losa cim se kreces izmedju klasa jer ne bi trebao nikad da diras UI prozora iz drugog prozora u back endu!!!!
    // iz tog razloga mora da se bar shvati generalna ideja viewmodela pa implementirati na pravilan nacin, za sada rade hardcode stvari
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // TODO: wont need this class in future so i wont refactor it 
    public class IpAddress
    {
        public String Local { get; set; } = "9.0.0.1";
        public String Public1 { get; set; } = "11.12.13.14";
        public String Public2 { get; set; } = "11.55.13.12";
        public String Public3 { get; set; } = "11.31.13.14";
    };

    public partial class MainWindow : Window
    {
        private IpAddress _ip = new IpAddress();
        public List<RoomView> RoomViewItems = new List<RoomView>();

        private App _bfmeApp;

        public MainWindow()
        {
            InitializeComponent();

            _bfmeApp = (App)Application.Current;
            if (_bfmeApp == null)
                throw new Exception("This is fatal. Should never happen.");

            //DataContext = new MainWindowViewModel(this);

            //roomViewList.ItemsSource = RoomViewItems;

            //dummy data
            //RoomViewItems.Add(new RoomView() { RoomName = "Enter for fun", Capacity = "2/8", Description = "Noobs only" });
            //RoomViewItems.Add(new RoomView() { RoomName = "FastEther", Capacity = "6/8", Description = "open room" });
            //RoomViewItems.Add(new RoomView() { RoomName = "Mordor", Capacity = "1/8", Description = "the shire" });
            //RoomViewItems.Add(new RoomView() { RoomName = "Guzlers", Capacity = "7/8", Description = "pro playz" });


            DataContext = _ip;
        }
        //need to refactor this via command

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            CreateRoom cr = new CreateRoom();
            cr.Show();

            //_bfmeApp.vpnProcess.ExecuteCommand("START", (res) =>
            //{
            //    Logger.LogMessage(res);
            //});
        }

        //should shitdown all apps when main window is closed
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void memscanButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO 
            MemoryHijacker.OpenProcess("game.dat");
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            var arguments = new Dictionary<string, string>() { { "p_ipaddr", _ip.Public1 } };

            _bfmeApp.vpnProcess.SetData(arguments, (res) => { Logger.LogMessage(res); });

            arguments["p_ipaddr"] = _ip.Public2;

            _bfmeApp.vpnProcess.SetData(arguments, (res) => { Logger.LogMessage(res); });

            arguments["p_ipaddr"] = _ip.Public3;

            _bfmeApp.vpnProcess.SetData(arguments, (res) => { Logger.LogMessage(res); });

            var arguments2 = new Dictionary<string, string>();
            arguments2.Add("ipaddr", _ip.Local);

            _bfmeApp.vpnProcess.SetData(arguments2, (res) => { Logger.LogMessage(res); });

            _bfmeApp.vpnProcess.ExecuteCommand("SAVE_CONFIG", (res) =>
            {
                Logger.LogMessage(res);
                
                // After config has been saved, we want to start
                _bfmeApp.vpnProcess.ExecuteCommand("START", (res) => { Logger.LogMessage(res); });
            });
        }
    }
}
