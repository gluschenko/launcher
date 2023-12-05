using System.Runtime.Serialization;
using System.Windows;

namespace Launcher
{
    [DataContract]
    public class Prefs
    {
        [DataMember] public double Width;
        [DataMember] public double Height;
        [DataMember] public WindowState WindowState;
        [DataMember] public string DefaultVersion;
        [DataMember] public string DefaultVersionPath;
    }
}
