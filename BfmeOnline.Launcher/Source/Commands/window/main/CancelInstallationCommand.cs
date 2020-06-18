using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.Commands.window.main
{
    class CancelInstallationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            logger.Logger.LogMessage("Cancelling...", "INSTALLER", logger.Logger.LogType.Warning);
            GameInstaller.Installer.Cancel();
        }
    }
}
