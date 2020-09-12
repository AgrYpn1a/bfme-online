using BfmeOnline.Launcher.Source.model;
using BfmeOnline.Launcher.Source.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.Commands.window.main
{
    class BrowseInstallPathCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MainModel model = (MainModel)parameter;
            logger.Logger.LogMessage($"mainVM {model.InstallPath}");

            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.ShowDialog();

            // Update path
            model.InstallPath = dlg.SelectedPath;
        }
    }
}
