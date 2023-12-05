using System.ComponentModel;
using System.IO;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using Launcher.Core;
using Launcher.Pages;

namespace Launcher.Views
{
    public partial class MainWindow : Window
    {
        // public
        public TabHost TabControl { get; private set; }

        public Config Config { get; set; }
        public Prefs Prefs { get; set; }
        public VersionsResponse Versions { get; set; }

        private readonly MainPage _mainPage;
        private readonly LocalPage _localPage;
        private readonly DownloadsPage _downloadsPage;
        private readonly SettingsPage _settingsPage;

        // private
        private readonly DataManager<Config> _configManager = new(App.GetAbsolutePath(App.ConfigPath));
        private readonly DataManager<Prefs> _prefsManager = new(App.GetAbsolutePath(App.PrefsPath));

        public MainWindow(
            MainPage mainPage, 
            LocalPage localPage, 
            DownloadsPage downloadsPage,
            SettingsPage settingsPage
        )
        {
            InitializeComponent();

            _mainPage = mainPage;
            _localPage = localPage;
            _downloadsPage = downloadsPage;
            _settingsPage = settingsPage;

            Title = App.Title;
            Version.Content = $"v {App.Version}";

            try
            {
                if (!File.Exists(_configManager.Path))
                {
                    MessageHelper.Error($"File '{Path.GetFileName(_configManager.Path)}' does not exist!");
                    Close();
                    return;
                }

                Config = _configManager.Load();
                Prefs = _prefsManager.Load();
            }
            catch (Exception e)
            {
                MessageHelper.ThrowException(e);
                Close();
                return;
            }

            if (Config != null && Prefs != null)
            {
                Image.Load(Config.Logo);
                BackgroundImage.Load(Config.Background);
                Title = Config.Title ?? "";
                Width = Prefs.Width;
                Height = Prefs.Height;
                WindowState = Prefs.WindowState;

                TabControl = new TabHost(Tabs, FrameView, Tab, new Page[] { _mainPage, _localPage, _downloadsPage /*, SettingsPage*/ });

                GetVersionsFromWeb((response) =>
                {
                    Versions = response;
                    _downloadsPage.Update(response);
                });

                Closing += OnWindowClosing;
            }
            else
            {
                MessageHelper.Error("Config is empty: " + _configManager.Path);
                Close();
                return;
            }
        }

        private void OnWindowClosing(object? sender, CancelEventArgs args)
        {
            Prefs.Width = Width;
            Prefs.Height = Height;
            Prefs.WindowState = WindowState;
            _prefsManager.Save(Prefs, (ee) => MessageHelper.ThrowException(ee));
        }

        public void GetVersionsFromWeb(Action<VersionsResponse> onDone)
        {
            IWebClient webClient = new WebClientAsync();
            webClient.Get(
                Config.VersionsURL, 
                (message) =>
                {
                    try
                    {
                        WebClient.ProcessResponse(message,
                            (data) =>
                            {
                                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionsResponse>(data);
                                onDone?.Invoke(result);
                            }, (code) =>
                            {
                                MessageHelper.Alert("Error: " + code);
                            });
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ThrowException(ex);
                    }
                }, 
                MessageHelper.ThrowException
            );
        }

    }
}
