using BfmeOnline.Launcher.Source.core;
using BfmeOnline.Launcher.Source.model;
using System.Windows;

namespace BfmeOnline.Launcher.Source.ViewModel
{
    public class MainViewModel
    {
        public MainModel Model;

        public MainViewModel()
        {
            Model = new MainModel(this);

            Model.OnlinePlayers = 0;

            // Subscribe to events
            Core.Instance.OnLauncherStateChange += HandleStateChange;
        }

        ~MainViewModel()
        {
            // Unsubscribe from events
            Core.Instance.OnLauncherStateChange -= HandleStateChange;
        }

        private void HandleStateChange(LauncherState newState)
        {
            logger.Logger.LogMessage("State change");
            switch (newState)
            {
                case LauncherState.Default:
                    {
                        // Show
                        Model.ShowHome = Visibility.Visible;
                        Model.ShowUserTitleBar = Visibility.Visible;

                        Model.ShowInstall = Visibility.Collapsed;
                        Model.ShowGameNotInstalled = Visibility.Collapsed;
                        break;
                    }

                case LauncherState.Installing:
                    {
                        logger.Logger.LogMessage($"State change {newState.ToString()}");

                        Model.ShowInstall = Visibility.Visible;
                        Model.ShowHome = Visibility.Collapsed;
                        Model.ShowUserTitleBar = Visibility.Collapsed;
                        Model.ShowGameNotInstalled = Visibility.Collapsed;
                        break;
                    }

                case LauncherState.GameNotInstalled:
                    {
                        logger.Logger.LogMessage($"State change {newState.ToString()}");

                        Model.ShowInstall = Visibility.Collapsed;
                        Model.ShowHome = Visibility.Collapsed;
                        Model.ShowUserTitleBar = Visibility.Collapsed;
                        Model.ShowGameNotInstalled = Visibility.Visible;
                        break;
                    }

            }
        }
    }
}
