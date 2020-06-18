namespace BfmeOnline.Launcher.Source.core
{
    public enum LauncherState
    {
        // Shows home screen
        Default = 0,

        Installing,

        // Shows launcher customization screen
        Settings,

        // Shows currently selected game
        Game,

        // Screen for finding quick matches
        Matchmaking
    }
}
