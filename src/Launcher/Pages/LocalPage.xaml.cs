using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Launcher.Helpers;
using Launcher.Models;
using Launcher.Services;
using Microsoft.Extensions.Hosting;

namespace Launcher.Pages
{
    public partial class LocalPage : Page, ITabPage
    {
        private readonly StateManager _stateManager;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        private readonly ObservableCollection<LocalBuild> _builds = new();

        public LocalPage(StateManager stateManager, IHostApplicationLifetime hostApplicationLifetime)
        {
            InitializeComponent();

            _stateManager = stateManager;
            _hostApplicationLifetime = hostApplicationLifetime;
            //
            ListView.Items.Clear();
            ListView.ItemsSource = _builds;

            _builds.CollectionChanged += (sender, e) =>
            {
                ListView.Items.Refresh();
                ListView.UpdateLayout();
            };
        }

        public void Update()
        {
            NoDataLabel.Visibility = Visibility.Hidden;

            var path = App.GetAbsolutePath(App.VersionsDirectory);

            if (Directory.Exists(path))
            {
                var dirs = Directory.GetDirectories(path);
                if (dirs.Length > 0)
                {
                    _builds.Clear();

                    for (var i = 0; i < dirs.Length; i++)
                    {
                        var title = Path.GetFileName(dirs[i]);
                        _builds.Add(new LocalBuild
                        {
                            Title = title,
                            Path = dirs[i],
                        });
                    }

                    return;
                }
            }

            NoDataLabel.Visibility = Visibility.Visible;
        }

        public void OnShown()
        {
            Update();
        }

        public void OnHidden()
        {
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var basePath = button.Tag.ToString() ?? throw new Exception("Path is null");

            var config = _stateManager.GetConfig();
            var prefs = _stateManager.GetPrefs();
            var path = Path.Combine(basePath, config.BuildExecutable);

            try
            {
                Process.Start(path);
                prefs.DefaultVersionPath = path;
                prefs.DefaultVersion = Path.GetFileName(basePath);
                _hostApplicationLifetime.StopApplication();
            }
            catch (Exception ex)
            {
                MessageHelper.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
            }
        }
    }
}
