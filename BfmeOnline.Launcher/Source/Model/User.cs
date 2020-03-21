using System;
using System.Collections.Generic;
using System.Text;

namespace BfmeOnline.Launcher.Source.Model
{
    [System.Serializable]
    public sealed class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Rank { get; set; }
    }
}
