using System.Runtime.Serialization;

namespace Launcher.Core
{
    [DataContract]
    public class VersionsResponse
    {
        [DataMember] public string Version = "";
        [DataMember] public string URL = "";
        [DataMember] public string InstallerURL = "";
        [DataMember] public string[] WindowsVersions = new string[0];
        [DataMember] public string[] WindowsBuilds = new string[0];
    }
}
