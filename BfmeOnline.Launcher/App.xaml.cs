using BfmeOnline.Launcher.Source;
using BfmeOnline.Launcher.Source.Auth;
using BfmeOnline.Launcher.Source.logger;
using BfmeOnline.Launcher.Source.Updates;
using BfmeOnline.Launcher.Source.WS;
using BfmeOnline.Launcher.View;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocket4Net;

namespace BfmeOnline.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Process pVPNManager;
        public InterProcessCommunicator vpnProcess;
        public LogWindow lw;
        public SignInWindow siw;

        public event Action<int> OnOnlinePlayersChanged;
        public event Action OnQueued;
        public event Action OnMatchFound;

        private Updater _winUpdater = new Updater();

        #region App Events

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (DebugWindows()) return;


            //bool hasUpdates = await UpdateManager.CheckForUpdates();
            //if (hasUpdates)
            //{
            //    // Bind progress watcher
            //    UpdateManager.OnDownloadProgressChange += progress =>
            //    {
            //        _winUpdater.SetDownloadProgress(progress);
            //    };

            //    _winUpdater.Show();
            //    _winUpdater.SetMessage("Downloading updates...");

            //    // Wait for updates to finish
            //    await UpdateManager.DownloadUpdates(err => { });

            //    Logger.LogMessage("Updates finished installing.");

            //    // Restart the app
            //    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //    Application.Current.Shutdown();
            //}

            // Bind event handlers
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            //TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


            //UpdateManager.OnDownloadProgressChange += progress =>
            //{
            //    _winUpdater.SetDownloadProgress(progress);
            //};

            //UpdateManager.OnUpdateFound += () =>
            //{
            //    _winUpdater.Show();
            //    _winUpdater.SetMessage("Downloading updates...");
            //};

            //UpdateManager.OnUpdateNotFound += () =>
            //{
            //    // Actually start the app
            //    new Main().Show();
            //};

            //UpdateManager.OnUpdateFinishedDownloading += () =>
            //{
            //    Logger.LogMessage("Updates finished downloading.");
            //};

            //UpdateManager.OnUpdatesBeginInstalling += () =>
            //{
            //    _winUpdater.SetMessage("Installing updates, please wait...");
            //};

            //UpdateManager.OnUpdatesFinshedInstalling += () =>
            //{
            //    Logger.LogMessage("Updates finished installing.");

            //    //_winUpdater.Close();
            //    //lw.Close();

            //    //Application.Current.Shutdown();
            //    //System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);

            //    //System.Windows.Form.Application.Restart();
            //    //System.Windows.Application.Current.Shutdown();

            //    //Application.Current.Shutdown();

            //    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //    Application.Current.Shutdown();

            //    //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //};

            lw = new LogWindow();
            lw.Show();

            new Main().Show();

            //await UpdateManager.BeginUpdating();

            //Logger.LogMessage("Checking for updates finished.");

            // Check for updates
            //if (await UpdateManager.CheckForUpdates())
            //{
            //    // Show dl window
            //    new Updater().Show();

            //    UpdateManager.DownloadUpdates((err) =>
            //    {
            //        MessageBox.Show("Error downloading updates. Message: " + err);
            //    }, () =>
            //    {
            //        MessageBox.Show("Download complete.");

            //        Process process = new Process();
            //        process.StartInfo.FileName = "msiexec.exe";
            //        process.StartInfo.Arguments = string.Format("/passive /i \"{0}\" ALLUSERS=1", System.IO.Path.Combine(System.IO.Path.GetTempPath(), "BfmeOnline.Launcher_Installer.msi"));
            //        process.Start();
            //        Application.Current.Shutdown();
            //    });
            //}
            //else
            //{
            //    lw = new LogWindow();
            //    lw.Show();

            //    WS.Instance.RegisterHandler(WSEvent.GETTER, message =>
            //    {
            //        Logger.LogMessage("GETTER");

            //        switch (message.ResultType)
            //        {
            //            case WSResult.ONLINE_PLAYERS:
            //                OnOnlinePlayersChanged?.Invoke(int.Parse(message.Message));
            //                break;
            //            case WSResult.QUEUED:
            //                OnQueued?.Invoke();
            //                break;
            //            case WSResult.MATCH_FOUND:
            //                OnMatchFound?.Invoke();
            //                break;
            //        }
            //    });

            //    WS.Instance.RegisterHandler(WSEvent.CONNECTION_OPENED, message =>
            //    {

            //        // Fetch info
            //        WS.Instance.SendMessage(new WSMessage() { EventType = WSEvent.GETTER, ResultType = WSResult.ONLINE_PLAYERS });

            //    });

            //    CheckForAuth();
            //}
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            string err;

            if (vpnProcess != null)
                vpnProcess.ExecuteCommand("SHUTDOWN", (res) => { });
        }

        private bool DebugWindows()
        {
#if DEBUG_MAIN_WIN
            new Main().Show();
            return true;
#elif DEBUG_OPTIONS_WIN
            new Options().Show();
            return true;
#endif
            return false;
        }

        #endregion

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        public event Action OnAuthorized;

        public async void CheckForAuth()
        {
            try
            {
                bool isAuth = await AuthManager.Instance.IsAuth();

                if (isAuth)
                {
                    OnAuthorized?.Invoke();

                    // Establish websocket connection
                    WS.Instance.EstablishWebSocketConnection();

                    // User logged in
                    RunLauncher();
                }
                else
                {
                    // Failed to login
                    RunLogin();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not authorize.");
            }
            finally
            {
                // TODO this is for testing only
                //new Main().Show();
            }
        }

        public WebSocket GetWS { get { return _ws; } private set { } }
        private WebSocket _ws;

        public void RunLauncher()
        {
            // TEST CODE
            new Main().Show();

            return;

            new MainWindow().Show();


            pVPNManager = new Process();
            pVPNManager.StartInfo.FileName = "vpn.exe";

            //hides the console
            pVPNManager.StartInfo.CreateNoWindow = true;

            vpnProcess = new InterProcessCommunicator(pVPNManager, (msg) =>
            {
                // Just log default output
                Logger.LogMessage(msg, "VPN OUT");
            });

            Thread t = new Thread(new ThreadStart(vpnProcess.Run));

            t.IsBackground = true;
            t.Start();
        }

        private void RunLogin()
        {
            siw = new SignInWindow();
            siw.Show();
        }
    }
}
