using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BfmeOnline.GameInstaller
{
    public enum InstallerState
    {
        NONE,
        DOWNLOADING,
        EXTRACTING,
        INSTALLING,
        FINISHED
    }

    public static class Installer
    {
        static Installer()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static InstallerState State { get; private set; } = InstallerState.NONE;
        public static int Progress { get; private set; } = 0;
        public static bool Finished { get; private set; } = false;

        private static object _root = new object();

        public static void Install(string downloadUrl, string installPath = @"E:\Program Files (x86)\EA GAMES\The Battle for Middle-earth\")
        {
            string downloadPath = $"{Path.GetTempPath()}bfme1.zip";
            State = InstallerState.DOWNLOADING;

            // First download files
            Downloader.Downloader dl = new Downloader.Downloader();
            Progress = 0;

            dl.OnProgressUpdate = progress =>
            {
                Progress = progress;
            };

            dl.OnDownloadFinished = () =>
            {
                lock (_root)
                {
                    State = InstallerState.EXTRACTING;
                }

                Task.Run(() =>
                {
                    ExtractFiles(downloadPath, installPath);

                    lock (_root)
                    {
                        State = InstallerState.INSTALLING;
                    }

                    InstallRegistryKeys(installPath);

                    lock (_root)
                    {
                        State = InstallerState.FINISHED;
                    }
                });
            };

            dl.DownloadFile(downloadUrl, downloadPath);
        }

        internal static void InstallRegistryKeys(string installPath)
        {
            RegistryKey electronicArtsKey = Registry.LocalMachine
                                        .OpenSubKey("SOFTWARE", true)
                                        .OpenSubKey("WOW6432Node", true)
                                        .CreateSubKey("Electronic Arts")
                                        .CreateSubKey("EA Games")
                                        .CreateSubKey("The Battle for Middle-earth");

            electronicArtsKey.SetValue("Language", "english", RegistryValueKind.String);
            electronicArtsKey.SetValue("InstallPath", installPath, RegistryValueKind.String);
            electronicArtsKey.SetValue("UserDataLeafName", "My Battle for Middle-earth Files", RegistryValueKind.String);

            electronicArtsKey.SetValue("Version", 0x00010000, RegistryValueKind.DWord);
            electronicArtsKey.SetValue("MapPackVersion", 0x00010000, RegistryValueKind.DWord);
            electronicArtsKey.SetValue("UseLocalUserMaps", 0x00000000, RegistryValueKind.DWord);

            // Add ergc (serial number)
            RegistryKey ergc = Registry.LocalMachine
                                        .OpenSubKey("Software", true)
                                        .OpenSubKey("WOW6432Node", true)
                                        .OpenSubKey("Electronic Arts", true)
                                        .OpenSubKey("EA Games", true)
                                        .OpenSubKey("The Battle for Middle-earth", true)
                                        .CreateSubKey("ergc");
            ergc.SetValue("@", Guid.NewGuid(), RegistryValueKind.String);

            RegistryKey bfmeKey = Registry.LocalMachine
                            .OpenSubKey("Software", true)
                            .OpenSubKey("WOW6432Node", true)
                            .CreateSubKey("EA Games")
                            .CreateSubKey("The Battle for Middle-earth");

            bfmeKey.SetValue("DisplayName", "The Battle for Middle-earth (tm)", RegistryValueKind.String);
            bfmeKey.SetValue("Installed From", @"C:\", RegistryValueKind.String);
            bfmeKey.SetValue("Registration", @"SOFTWARE\Electronic Arts\EA GAMES\The Battle for Middle-earth\ergc", RegistryValueKind.String);
            bfmeKey.SetValue("CacheSize", "3351006208", RegistryValueKind.String);
            bfmeKey.SetValue("SwapSize", "0", RegistryValueKind.String);
            bfmeKey.SetValue("Language", "English US", RegistryValueKind.String);
            bfmeKey.SetValue("Locale", "en_us", RegistryValueKind.String);
            bfmeKey.SetValue("CD Drive", @"E:\", RegistryValueKind.String);
            bfmeKey.SetValue("Install Dir", installPath, RegistryValueKind.String);
            bfmeKey.SetValue("Product GUID", "{3F290582-3F4E-4B96-009C-E0BABAA40C42}", RegistryValueKind.String);
            bfmeKey.SetValue("Region", "Europe", RegistryValueKind.String);
            bfmeKey.SetValue("Folder", @"C:\Documents and Settings\All Users\Start Menu\Programs\EA GAMES\The Battle for Middle-earth (tm)\", RegistryValueKind.String);
            bfmeKey.SetValue("Patch URL", "http://transtest.ea.com/Electronic Arts/The Battle for Middle-earth/Europe", RegistryValueKind.String);
            bfmeKey.SetValue("Suppression Exe", "rtsi.exe", RegistryValueKind.String);

            RegistryKey bfme1Key = Registry.LocalMachine
                .OpenSubKey("Software", true)
                .OpenSubKey("Wow6432Node", true)
                .OpenSubKey("EA Games", true)
                .OpenSubKey("The Battle for Middle-earth", true)
                .CreateSubKey("1.0");

            bfme1Key.SetValue("Language", 0x00000001, RegistryValueKind.DWord);
            bfme1Key.SetValue("DisplayName", "The Battle for Middle-earth (tm)", RegistryValueKind.String);
            bfme1Key.SetValue("LanguageName", "English US", RegistryValueKind.String);
        }

        public static void ExtractFiles(string filePath, string installPath)
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(filePath))
            {
                zip.ExtractProgress += Zip_ExtractProgress;
                zip.Password = "X4Ax6w2RN9zgrTVZUt7xEZpYG75kaqfz";
                zip.ExtractAll(installPath, ExtractExistingFileAction.OverwriteSilently);
            }

            File.Delete(filePath);

            Finished = true;
        }

        private static void Zip_ExtractProgress(object sender, Ionic.Zip.ExtractProgressEventArgs e)
        {
            if (e.TotalBytesToTransfer == 0)
                return;

            lock (_root)
            {
                Progress = Convert.ToInt32(100 * e.BytesTransferred / e.TotalBytesToTransfer);
            }

        }
    }
}
