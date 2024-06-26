﻿using BfmeOnline.Launcher.Source.Http;
using BfmeOnline.Launcher.Source.logger;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace BfmeOnline.Launcher.Source.Updates
{
    sealed class LauncherUpdateManager : IUpdateManager
    {
        private static readonly string DOWNLOAD_PATH = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
            "BfmeOnline.Launcher_Installer.msi");

        public static async Task BeginUpdating()
        {
            if (await CheckForUpdates())
            {
                OnUpdateFound?.Invoke();
                await DownloadUpdates(error =>
                {
                    Logger.LogMessage("Error downloading updates.");
                    Logger.LogMessage(error);
                });
            }
            else
            {
                OnUpdateNotFound?.Invoke();
            }
        }

        public static async Task<bool> CheckForUpdates()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(NetworkAddresses.LAUNCHER_VERSION);
                    string body = await result.Content.ReadAsStringAsync();

                    Model.ND_LauncherVersion response = JsonConvert.DeserializeObject<Model.ND_LauncherVersion>(body);
                    AppVersion onlineVersion = new AppVersion(response.Major, response.Minor, response.Patch);
                    AppVersion localVersion = Util.GetLocalVersion();

                    return !onlineVersion.Equals(localVersion);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
                catch (JsonReaderException e)
                {
                    MessageBox.Show("Something wrong with data received from the server.");
                    return false;
                }
            }
        }

        private static Action _dlCompleteAction;

        public static async Task DownloadUpdates(Action<string> errCallback)
        {
            using (var client = new WebClient())
            {
                // Bind events
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                // Begin download
                try
                {
                    await client.DownloadFileTaskAsync(new Uri(NetworkAddresses.LAUNCHER_UPDATE), DOWNLOAD_PATH);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void InstallUpdates()
        {
            Process process = new Process();
            process.StartInfo.FileName = "msiexec.exe";
            process.StartInfo.Arguments = string.Format("/passive /i \"{0}\" ALLUSERS=1", System.IO.Path.Combine(System.IO.Path.GetTempPath(), "BfmeOnline.Launcher_Installer.msi"));
            process.Exited += Process_Exited;
            process.EnableRaisingEvents = true;

            OnUpdatesBeginInstalling?.Invoke();

            process.Start();
            process.WaitForExit();
            process.Close();
        }

        private static void Process_Exited(object sender, EventArgs e)
        {
            OnUpdatesFinshedInstalling?.Invoke();
        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnDownloadProgressChange?.Invoke(e.ProgressPercentage);
        }

        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            OnUpdateFinishedDownloading?.Invoke();
            InstallUpdates();
        }

        public async Task<bool> HasUpdates()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(NetworkAddresses.LAUNCHER_VERSION);
                    string body = await result.Content.ReadAsStringAsync();

                    Model.ND_LauncherVersion response = JsonConvert.DeserializeObject<Model.ND_LauncherVersion>(body);
                    AppVersion onlineVersion = new AppVersion(response.Major, response.Minor, response.Patch);
                    AppVersion localVersion = Util.GetLocalVersion();

                    return !onlineVersion.Equals(localVersion);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
                catch (JsonReaderException e)
                {
                    MessageBox.Show("Something wrong with data received from the server.");
                    return false;
                }
            }
        }

        public async Task Update()
        {
            // Download updates
            using (var client = new WebClient())
            {
                // Bind events
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                // Begin download
                try
                {
                    await client.DownloadFileTaskAsync(new Uri(NetworkAddresses.LAUNCHER_UPDATE), DOWNLOAD_PATH);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            // Install updates
            Process process = new Process();
            process.StartInfo.FileName = "msiexec.exe";
            process.StartInfo.Arguments = string.Format("/passive /i \"{0}\" ALLUSERS=1", System.IO.Path.Combine(System.IO.Path.GetTempPath(), "BfmeOnline.Launcher_Installer.msi"));
            process.Exited += Process_Exited;
            process.EnableRaisingEvents = true;

            OnUpdatesBeginInstalling?.Invoke();

            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public static Action<int> OnDownloadProgressChange;

        public static event Action OnUpdateFound;
        public static event Action OnUpdateNotFound;
        public static event Action OnUpdateFinishedDownloading;
        public static event Action OnUpdatesFinshedInstalling;
        public static event Action OnUpdatesBeginInstalling;
    }
}
