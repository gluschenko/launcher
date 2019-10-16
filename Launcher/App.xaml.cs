using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher
{
    public partial class App : Application
    {
        public const string Title = "Launcher";
        public const string Version = "0.1";
        public const string ConfigPath = "Launcher.json";
        public const string DownloadsDirectory = "Downloads";
        public const string VersionsDirectory = "Versions";
    }
}
