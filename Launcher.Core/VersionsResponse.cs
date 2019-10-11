using System;
using System.Collections.Generic;

namespace Launcher.Core
{
    public class VersionsResponse
    {
        public string Version;
	    public string URL;
	    public string InstallerURL;
	    public Dictionary<string, string> WindowsBuilds;
    }
}
