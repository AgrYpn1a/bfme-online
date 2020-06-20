using BfmeOnline.Launcher.Source.core;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace BfmeOnline.Launcher.Source.commands.window.main
{
    class SelectGameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string btnName = (parameter as string);
            logger.Logger.LogMessage(btnName, "Button");

            switch (btnName)
            {
                case "bfme1":
                    {
                        logger.Logger.LogMessage("Bfme 1 Selected", "Button");
                        //Core.Instance.ChangeState(LauncherState.Game);
                        DetermineGameState(Game.Bfme1);
                        break;
                    }
                default: break;
            }
        }

        private void DetermineGameState(Game game)
        {
            switch (game)
            {
                case Game.Bfme1:
                    {
                        if (Core.Instance.IsGameInstalled(game))
                        {
                            // Show main game screen
                            Core.Instance.ChangeState(LauncherState.Game);
                        }
                        else
                        {
                            // Show installation screen
                            Core.Instance.ChangeState(LauncherState.GameNotInstalled);
                        }

                        break;
                    }
            }
        }
    }
}
