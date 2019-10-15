using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
