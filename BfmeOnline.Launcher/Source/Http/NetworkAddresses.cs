namespace BfmeOnline.Launcher.Source.Http
{
    public static class NetworkAddresses
    {
        public static readonly string ADMIN = "http://admin-api.thebfmeonline.com";
        public static readonly string ADMIN_VERSION = $"{ADMIN}/version";
        public static readonly string ADMIN_DOWNLOAD = $"{ADMIN}/download";

        public static readonly string BFME_DOWNLOAD = $"{ADMIN}/download-game";

        // Game update
        public static readonly string GAME_UPDATE_VERSION   = $"{ADMIN}/api/game/version";
        public static readonly string GAME_UPDATE_DOWNLOAD  = $"{ADMIN}/api/game/download";

        
        public static readonly string LAUNCHER_UPDATE = $"{ADMIN}/api/release/download";
        public static readonly string LAUNCHER_VERSION = $"{ADMIN}/api/release/version";
    }
}
