using BfmeOnline.Launcher.logger;
using BfmeOnline.Launcher.logger.model;
using BfmeOnline.Launcher.View;
using BfmeOnline.Launcher.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BfmeOnline.Launcher.Source.logger
{
    /// <summary>
    /// Use this class to output log instead Console.WriteLine.
    /// </summary>
    public static class Logger
    {
        public static LogWindowViewModel lwvm;
        public static LogTab lt;

        private static object _root = new object();

        public enum LogType
        {
            Default,
            Warning,
            Error
        }

        /// <summary>
        /// Use this to name log tabs.
        /// </summary>
        public static Dictionary<LogType, string> logNames = new Dictionary<LogType, string>()
        {
            { LogType.Default, "Log"},
            { LogType.Warning, "Warnings"},
            { LogType.Error, "Errors"}
        };

        public static string UppercaseFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        public static void LogMessage(string message, string id = "INFO", LogType type = LogType.Default)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Tab targetTab = lwvm.Tabs.First(t => t.Name.Equals(logNames[type])) as Tab;

                // Found the tab
                if (targetTab != null)
                {
                    targetTab.Content +=
                        $"[{DateTime.Now.ToString("HH:mm:ss")}] [{id}] {message}{System.Environment.NewLine}";

                    LogWindow logWindow = App.Current.Windows.OfType<LogWindow>().First();

                    Tab currTab = ((Tab)logWindow.tbc.SelectedItem);
                    if (!currTab.Name.Equals(targetTab.Name))
                    {
                        // Find index
                        logWindow.tbc.SelectedItem = targetTab;
                    }
                    else
                    {
                        // Refresh currently selected tab's content
                        var index = logWindow.tbc.SelectedIndex;
                        logWindow.tbc.SelectedIndex = (index + 1) % logWindow.tbc.Items.Count;
                        logWindow.tbc.SelectedIndex = index;
                    }
                }

            });
        }
    }
}
