using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        readonly ObservableCollection<Build> Builds = new ObservableCollection<Build>();

        public DownloadsPage()
        {
            InitializeComponent();
            //
            ListView.Items.Clear();
            ListView.ItemsSource = Builds;

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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var basePath = button.Tag.ToString();

            MessageBox.Show(basePath);

            foreach (var build in Builds)
            {
                build.Title += "123";
            }

            UpdateItems();
        }
    }
}
