using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Windows.Shapes;
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
            listBox.Items.Clear();
            listBox.ItemsSource = Builds;

            Builds.CollectionChanged += (sender, e) =>
            {
                listBox.Items.Refresh();
                listBox.UpdateLayout();
            };
        }

        public void Update(VersionsResponse response)
        {
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

                /*listBox.Items.Clear();
                listBox.ItemsSource = Builds;
                listBox.Items.Refresh();
                listBox.UpdateLayout();*/
            }
            else
            {

            }
        }

        public void OnShown()
        {

        }

        public void OnHidden()
        {
            
        }
    }
}
