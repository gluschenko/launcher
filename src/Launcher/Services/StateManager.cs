using System.IO;
using Launcher.Core;
using Launcher.Models;

namespace Launcher.Services
{
    public class StateManager
    {
        private readonly DataManager<Config> _configManager;
        private readonly DataManager<Prefs> _prefsManager;

        private Config? _config;
        private Prefs? _prefs;
        private TabHost? _tabHost;
        private VersionsResponse? _versions;

        public StateManager(
            DataManager<Config> configManager,
            DataManager<Prefs> prefsManager
        )
        {
            _configManager = configManager;
            _prefsManager = prefsManager;
        }

        public Config LoadConfig()
        {
            try
            {
                _config = _configManager.Load();
                return _config;
            }
            catch (Exception ex)
            {
                throw new Exception($"File '{Path.GetFileName(_configManager.Path)}' does not exist!", ex);
            }
        }

        public Prefs LoadPrefs()
        {
            try
            {
                _prefs = _prefsManager.Load();
                return _prefs;
            }
            catch (Exception ex)
            {
                throw new Exception($"File '{Path.GetFileName(_prefsManager.Path)}' does not exist!", ex);
            }
        }

        public void SavePrefs()
        {
            if (_prefs is null)
            {
                throw new Exception("Prefs is not loaded yet");
            }

            _prefsManager.Save(_prefs, (ex) =>
            {
                throw ex;
            });
        }

        public Config GetConfig()
        {
            if (_config is null)
            {
                throw new Exception($"{nameof(Config)} is not loaded yet");
            }

            return _config;
        }

        public Prefs GetPrefs()
        {
            if (_prefs is null)
            {
                throw new Exception($"{nameof(Prefs)} is not loaded yet");
            }

            return _prefs;
        }

        public TabHost SetTabHost(TabHost tabHost)
        {
            _tabHost = tabHost;
            return _tabHost;
        }

        public TabHost GetTabHost()
        {
            if (_tabHost is null)
            {
                throw new Exception($"{nameof(TabHost)} is not set");
            }

            return _tabHost;
        }

        public VersionsResponse SetVersions(VersionsResponse versions)
        {
            _versions = versions;
            return _versions;
        }

        public VersionsResponse GetVersions()
        {
            if (_versions is null)
            {
                throw new Exception($"{nameof(VersionsResponse)} is not set");
            }

            return _versions;
        }
    }
}
