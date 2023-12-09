using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Launcher.Core;
using Launcher.Helpers;
using Launcher.Pages;
using Launcher.Services;

namespace Launcher.Views
{
    public partial class MainWindow : Window
    {
        private readonly StateManager _stateManager;

        private readonly MainPage _mainPage;
        private readonly LocalPage _localPage;
        private readonly DownloadsPage _downloadsPage;
        private readonly SettingsPage _settingsPage;

        public MainWindow(
            StateManager stateManager,
            MainPage mainPage,
            LocalPage localPage,
            DownloadsPage downloadsPage,
            SettingsPage settingsPage
        )
        {
            InitializeComponent();
            Title = App.Title;
            Version.Content = $"v {App.Version}";

            _stateManager = stateManager;
            _mainPage = mainPage;
            _localPage = localPage;
            _downloadsPage = downloadsPage;
            _settingsPage = settingsPage;

            var config = _stateManager.LoadConfig();
            var prefs = _stateManager.LoadPrefs();

            Image.Load(config.Logo);
            BackgroundImage.Load(config.Background);
            Title = config.Title ?? "";
            Width = prefs.Width;
            Height = prefs.Height;
            WindowState = prefs.WindowState;

            _stateManager.SetTabHost(
                new TabHost(Tabs, FrameView, Tab, new Page[] { _mainPage, _localPage, _downloadsPage })
            );

            GetVersionsFromWeb((response) =>
            {
                _stateManager.SetVersions(response);
                _downloadsPage.Update(response);
            });

            Closing += OnWindowClosing;
        }

        private void OnWindowClosing(object? sender, CancelEventArgs args)
        {
            var prefs = _stateManager.GetPrefs();

            prefs.Width = Width;
            prefs.Height = Height;
            prefs.WindowState = WindowState;

            try
            {
                _stateManager.SavePrefs();
            }
            catch (Exception ex)
            {
                MessageHelper.ThrowException(ex);
            }
        }

        public void GetVersionsFromWeb(Action<VersionsResponse> onDone)
        {
            var config = _stateManager.GetConfig();

            IWebClient webClient = new WebClientAsync();
            webClient.Get(
                config.VersionsURL,
                (message) =>
                {
                    try
                    {
                        WebClient.ProcessResponse(message,
                            (data) =>
                            {
                                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionsResponse>(data);

                                if (result is not null)
                                {
                                    onDone?.Invoke(result);
                                }
                                else
                                {
                                    MessageHelper.Alert("Error: " + data);
                                }
                            },
                            (code) =>
                            {
                                MessageHelper.Alert("Error: " + code);
                            }
                        );
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
