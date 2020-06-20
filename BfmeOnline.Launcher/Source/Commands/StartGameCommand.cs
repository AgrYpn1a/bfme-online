using BfmeOnline.Launcher.Source.logger;
using System;
using System.Windows;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.Commands
{
    class StartGameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Logger.LogMessage("[CMD] StartGame");
            System.Diagnostics.Process.Start($"{RegistryManager.GetInstallPath()}/lotrbfme.exe");
        }
    }
}
