using System.Runtime.Serialization;

namespace Launcher
{
    [DataContract]
    public class Config
    {
        [DataMember] public string Title = "";
        [DataMember] public string Logo = "";
        [DataMember] public string Background = "";
        [DataMember] public string VersionsURL = "";
        [DataMember] public string BuildExecutable = "";
    }
}
