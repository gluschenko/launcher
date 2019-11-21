using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using WebClient = System.Net.WebClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using Launcher.Core;
using Launcher.Views;
using Launcher.Entities;

namespace Launcher.Pages
{
    public partial class DownloadsPage : Page, ITabPage
    {
        public MainWindow MainWindow;

        readonly ObservableCollection<Build> Builds = new ObservableCollection<Build>();

        public DownloadsPage()
        {
            InitializeComponent();
            //
            ListView.Items.Clear();
            ListView.ItemsSource = Builds;
            //
            ProgessBar.Visibility = Visibility.Hidden;

            Builds.CollectionChanged += (sender, e) =>
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
                    Builds.Clear();

                    for (int i = 0; i < versions.Length; i++)
                    {
                        Builds.Add(new Build { Title = versions[i], URL = builds[i] });
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

        private int curPercent = 0;
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var uri = button.Tag.ToString();
            var fileName = Path.GetFileName(uri);
            var destPath = App.GetAbsolutePath(App.DownloadsDirectory);
            destPath = Path.Combine(destPath, fileName);

            try 
            {
                if (!Directory.Exists(Path.GetDirectoryName(destPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                }

                Download(uri, destPath, onChange, onDone);

                void onChange(int percentage) 
                {
                    if (percentage != curPercent)
                    {
                        Dispatcher.Invoke(() => {
                            ProgessBar.Visibility = Visibility.Visible; 
                            ProgessBar.Value = percentage;

                             curPercent = percentage;
                        });
                    }
                }

                void onDone()
                {
                    ProgessBar.Visibility = Visibility.Hidden;

                    try
                    {
                        Process.Start(destPath);
                        //MainWindow.Close();
                    }
                    catch (Exception ex)
                    {
                        MainWindow.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private WebClient webClient = new WebClient();
        private async void Download(string uri, string dest, Action<int> onChange, Action onDone)
        {
            if (webClient.IsBusy) return;

            webClient.DownloadProgressChanged += (s, e) => onChange?.Invoke(e.ProgressPercentage);

            var data = await webClient.DownloadDataTaskAsync(new Uri(uri));

            using (var stream = new FileStream(dest, FileMode.OpenOrCreate)) 
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            onDone?.Invoke();
            GC.Collect();
        }
    }
}
