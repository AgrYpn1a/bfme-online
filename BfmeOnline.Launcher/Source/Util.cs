using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BfmeOnline.Launcher.Source
{
    [System.Serializable]
    struct AppVersion
    {
        public int major;
        public int minor;
        public int patch;

        public AppVersion(int major, int minor, int patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }

        public override string ToString() => $"{major}.{minor}.{patch}";

        public override bool Equals(object obj)
        {
            AppVersion other = (AppVersion)obj;
            return other.major == major && other.minor == minor && other.patch == patch;
        }
    }

    class Util
    {
        public static AppVersion GetLocalVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            string[] versionNumbers = fvi.FileVersion.Split('.');
            AppVersion version = new AppVersion(int.Parse(versionNumbers[0]), int.Parse(versionNumbers[1]), int.Parse(versionNumbers[2]));

            return version;
        }
    }
}
