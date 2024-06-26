﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BfmeOnline.Launcher.Source.Model
{
    [System.Serializable]
    public enum ResponseStatus
    {
        ERR = -1,
        OK = 0
    }

    [System.Serializable]
    public sealed class WebResponseData
    {
        public string Message { get; set; }
        public string[] Path { get; set; }
    }

    [System.Serializable]
    public sealed class WebResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public WebResponseData Data { get; set; }
        public User User { get; set; }
    }

    [System.Serializable]
    public sealed class ND_LauncherVersion
    {
        [JsonProperty("major")]
        public int Major { get; set; }
        [JsonProperty("minor")]
        public int Minor { get; set; }
        [JsonProperty("patch")]
        public int Patch { get; set; }
    }
}
