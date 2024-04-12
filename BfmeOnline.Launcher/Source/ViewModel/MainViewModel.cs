using BfmeOnline.Launcher.Source.core;
using BfmeOnline.Launcher.Source.model;
using BfmeOnline.Launcher.Source.viewmodel;
using System.Windows;

namespace BfmeOnline.Launcher.Source.ViewModel
{
    public class MainViewModel : AViewModel<MainViewModel, MainModel>
    {
        public MainViewModel(Window window) : base(window)
        {
            m_model.OnlinePlayers = 0;

            // Subscribe to events
            Core.Instance.OnLauncherStateChange += HandleStateChange;
        }

        ~MainViewModel()
        {
            // Unsubscribe from events
            Core.Instance.OnLauncherStateChange -= HandleStateChange;
        }

        protected override MainModel GetModel()
        {
            return new MainModel(this);
        }

        private void HandleStateChange(LauncherState newState)
        {
            logger.Logger.LogMessage("State change");
            switch (newState)
            {
                case LauncherState.Default:
                    {
                        // Show
                        m_model.ShowHome = Visibility.Visible;
                        m_model.ShowUserTitleBar = Visibility.Visible;

                        m_model.ShowInstall = Visibility.Collapsed;
                        m_model.ShowGameNotInstalled = Visibility.Collapsed;
                        break;
                    }

                case LauncherState.Game:
                    {
                        m_model.ShowPlayScreen = Visibility.Visible;

                        m_model.ShowHome = Visibility.Collapsed;
                        m_model.ShowUserTitleBar = Visibility.Collapsed;
                        m_model.ShowInstall = Visibility.Collapsed;
                        m_model.ShowGameNotInstalled = Visibility.Collapsed;
                        break;
                    }

                case LauncherState.Installing:
                    {
                        logger.Logger.LogMessage($"State change {newState.ToString()}");

                        m_model.ShowInstall = Visibility.Visible;
                        m_model.ShowHome = Visibility.Collapsed;
                        m_model.ShowUserTitleBar = Visibility.Collapsed;
                        m_model.ShowGameNotInstalled = Visibility.Collapsed;
                        break;
                    }

                case LauncherState.GameNotInstalled:
                    {
                        logger.Logger.LogMessage($"State change {newState.ToString()}");

                        m_model.ShowInstall = Visibility.Collapsed;
                        m_model.ShowHome = Visibility.Collapsed;
                        m_model.ShowUserTitleBar = Visibility.Collapsed;
                        m_model.ShowGameNotInstalled = Visibility.Visible;
                        break;
                    }

            }
        }
    }
}
