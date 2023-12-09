using System.Windows;

namespace Launcher.Models
{
    public class Prefs
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public WindowState WindowState { get; set; }
        public string DefaultVersion { get; set; } = null!;
        public string DefaultVersionPath { get; set; } = null!;
    }
}
