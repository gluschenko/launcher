namespace Launcher.Core
{
    public class VersionsResponse
    {
        public string Version { get; set; } = "";
        public string URL { get; set; } = "";
        public string InstallerURL { get; set; } = "";
        public string[] WindowsVersions { get; set; } = System.Array.Empty<string>();
        public string[] WindowsBuilds { get; set; } = System.Array.Empty<string>();
    }
}
