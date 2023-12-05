using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Launcher.Core;
using Launcher.Entities;
using WebClient = System.Net.WebClient;

namespace Launcher.Pages
{
    public partial class DownloadsPage : Page, ITabPage
    {
        private readonly ObservableCollection<Build> _builds = new();
        private readonly WebClient _webClient = new WebClient();
        private int _currentProgress = 0;

        public DownloadsPage()
        {
            InitializeComponent();
            //
            ListView.Items.Clear();
            ListView.ItemsSource = _builds;
            //
            ProgessBar.Visibility = Visibility.Hidden;

            _builds.CollectionChanged += (sender, e) =>
            {
                UpdateItems();
            };
        }

        public void Update(VersionsResponse response)
        {
            NoDataLabel.Visibility = Visibility.Hidden;

            if (response != null)
            {
                var versions = response.WindowsVersions;
                var builds = response.WindowsBuilds;

                if (versions.Length == builds.Length)
                {
                    _builds.Clear();

                    for (int i = 0; i < versions.Length; i++)
                    {
                        _builds.Add(new Build { Title = versions[i], URL = builds[i] });
                    }
                }
            }
            else
            {
                NoDataLabel.Visibility = Visibility.Visible;
            }
        }

        public void UpdateItems()
        {
            ListView.Items.Refresh();
            ListView.UpdateLayout();
        }

        public void OnShown()
        {

        }

        public void OnHidden()
        {

        }


        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var uri = button.Tag.ToString() ?? throw new Exception($"Uri is null");
            var fileName = Path.GetFileName(uri);
            var destPath = App.GetAbsolutePath(App.DownloadsDirectory);
            destPath = Path.Combine(destPath, fileName);

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(destPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                }

                Download(uri, destPath, OnChange, OnDone);

                void OnChange(int percentage)
                {
                    if (percentage != _currentProgress)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ProgessBar.Visibility = Visibility.Visible;
                            ProgessBar.Value = percentage;

                            _currentProgress = percentage;
                        });
                    }
                }

                void OnDone()
                {
                    ProgessBar.Visibility = Visibility.Hidden;

                    try
                    {
                        Process.Start(destPath);
                        //MainWindow.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void Download(string uri, string dest, Action<int> onChange, Action onDone)
        {
            if (_webClient.IsBusy) return;

            _webClient.DownloadProgressChanged += (s, e) => onChange?.Invoke(e.ProgressPercentage);

            var data = await _webClient.DownloadDataTaskAsync(new Uri(uri));

            using (var stream = new FileStream(dest, FileMode.OpenOrCreate))
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            onDone?.Invoke();
            GC.Collect();
        }
    }
}
