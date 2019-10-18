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
using Launcher.Views;
using Launcher.Entities;

namespace Launcher.Pages
{
    public partial class LocalPage : Page, ITabPage
    {
        public MainWindow MainWindow;

        readonly ObservableCollection<LocalBuild> Builds = new ObservableCollection<LocalBuild>();

        public LocalPage()
        {
            InitializeComponent();
            //
            ListView.Items.Clear();
            ListView.ItemsSource = Builds;

            Builds.CollectionChanged += (sender, e) =>
            {
                ListView.Items.Refresh();
                ListView.UpdateLayout();
            };
        }

        public void Update()
        {
            NoDataLabel.Visibility = Visibility.Hidden;

            string dir = App.GetAbsolutePath(App.VersionsDirectory);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Directory.Exists(dir))
            {
                string[] dirs = Directory.GetDirectories(dir);
                if (dirs.Length > 0)
                {
                    Builds.Clear();

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        string title = Path.GetFileName(dirs[i]);
                        Builds.Add(new LocalBuild { Title = title, Path = dirs[i] });
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
            var basePath = button.Tag.ToString();

            if (MainWindow?.Config != null)
            {
                string path = Path.Combine(basePath, MainWindow.Config.BuildExecutable);
                MessageBox.Show(path);
            }
        }
    }
}
