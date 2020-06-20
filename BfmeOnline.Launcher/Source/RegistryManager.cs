using BfmeOnline.Launcher.Source.logger;
using Microsoft.Win32;
using System;

namespace BfmeOnline.Launcher.Source
{
    internal class RegistryManager
    {
        public static bool IsGameInstalled()
        {
            string gamePath = GetInstallPath();

            if (gamePath != null)
            {
                LauncherData.bfmeGameInstallPath = gamePath;
                return true;
            }

            return false;
        }

        public static string GetInstallPath()
        {
            try
            {
                RegistryKey electronicArtsKey = Registry.LocalMachine
                    .OpenSubKey("SOFTWARE", true)
                    .OpenSubKey("WOW6432Node", true)
                    ?.OpenSubKey("Electronic Arts")
                    ?.OpenSubKey("EA Games")
                    ?.OpenSubKey("The Battle for Middle-earth");

                return electronicArtsKey?.GetValue("InstallPath", RegistryValueKind.String) as string;
            }
            catch (Exception e)
            {
                Logger.LogMessage(e.Message, "ERR", Logger.LogType.Error);
                return string.Empty;
            }
        }
    }
}
