using BfmeOnline.Launcher.Source.logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.commands.window.main
{
    class InstallGameCommand : ICommand
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
            return false;
        }

        public void Execute(object parameter)
        {
            Logger.LogMessage("[CMD] InstallGame");
            MessageBox.Show("[CMD] InstallGame");
        }
    }
}
