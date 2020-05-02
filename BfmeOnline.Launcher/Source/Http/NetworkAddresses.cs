﻿ using System;
using System.Collections.Generic;
using System.Text;

namespace BfmeOnline.Launcher.Source.Http
{
    public static class NetworkAddresses
    {
        public static readonly string ADMIN = "http://thebfmeonline";
        public static readonly string ADMIN_VERSION = $"{ADMIN}/version";
        public static readonly string ADMIN_DOWNLOAD = $"{ADMIN}/download";

        public static readonly string BFME_DOWNLOAD = $"{ADMIN}/download-game";
    }
}
