using BfmeOnline.Launcher.Source.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.Commands.window.main
{
    class BackToHomeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Core.Instance.ChangeState(LauncherState.Default);
        }
    }
}
