namespace BfmeOnline.Launcher.Source.core
{
    public enum LauncherState
    {
        // Shows home screen
        Default = 0,

        GameNotInstalled,

        Installing,

        // Shows launcher customization screen
        Settings,

        // Shows currently selected game
        Game,

        CheckingForGameUpdates,

        CheckingForUpdates,

        // Screen for finding quick matches
        Matchmaking
    }
}
