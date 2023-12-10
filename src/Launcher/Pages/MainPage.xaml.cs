using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Launcher.Helpers;
using Launcher.Services;
using Microsoft.Extensions.Hosting;

namespace Launcher.Pages
{
    public partial class MainPage : Page, ITabPage
    {
        private readonly StateManager _stateManager;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public MainPage(StateManager stateManager, IHostApplicationLifetime hostApplicationLifetime)
        {
            InitializeComponent();
            _stateManager = stateManager;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public void OnShown()
        {
            VersionLabel.Content = "";

            var prefs = _stateManager.GetPrefs();
            VersionLabel.Content = prefs.DefaultVersion ?? "";
        }

        public void OnHidden()
        {
        }

        private void PlayButton_Click(object? sender, RoutedEventArgs e)
        {
            var prefs = _stateManager.GetPrefs();


            if (string.IsNullOrEmpty(prefs.DefaultVersionPath))
            {
                _stateManager.GetTabHost().Navigate<LocalPage>();
            }
            else
            {
                try
                {
                    Process.Start(_stateManager.GetPrefs().DefaultVersionPath);
                    _hostApplicationLifetime.StopApplication();
                }
                catch (Exception ex)
                {
                    MessageHelper.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
                    _stateManager.GetTabHost().Navigate<LocalPage>();
                }
            }
        }
    }
}
