using System;
using System.IO;
using System.Windows;

namespace Launcher
{
    public partial class App : Application
    {
        public const string Title = "Launcher";
        public const string Version = "2019.11";
        public const string ConfigPath = "Launcher.json";
        public const string PrefsPath = "LauncherPrefs.json";
        public const string DownloadsDirectory = "Downloads";
        public const string VersionsDirectory = "Versions";

        public static string GetAbsolutePath(string directory) => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);
    }
}
