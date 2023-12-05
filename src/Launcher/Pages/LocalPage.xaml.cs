using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Launcher.Entities;
using Launcher.Views;

namespace Launcher.Pages
{
    public partial class LocalPage : Page, ITabPage
    {
        //private readonly MainWindow _mainWindow;

        private readonly ObservableCollection<LocalBuild> _builds = new();

        public LocalPage(/*MainWindow mainWindow*/)
        {
            InitializeComponent();
            //_mainWindow = mainWindow;
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

            string dir = App.GetAbsolutePath(App.VersionsDirectory);
            //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Directory.Exists(dir))
            {
                string[] dirs = Directory.GetDirectories(dir);
                if (dirs.Length > 0)
                {
                    _builds.Clear();

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        string title = Path.GetFileName(dirs[i]);
                        _builds.Add(new LocalBuild { Title = title, Path = dirs[i] });
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

            /*if (_mainWindow?.Config != null)
            {
                string path = Path.Combine(basePath, _mainWindow.Config.BuildExecutable);
                //MessageBox.Show(path);

                try
                {
                    Process.Start(path);
                    _mainWindow.Prefs.DefaultVersionPath = path;
                    _mainWindow.Prefs.DefaultVersion = Path.GetFileName(basePath);
                    _mainWindow.Close();
                }
                catch (Exception ex)
                {
                    MessageHelper.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
                }
            }*/
        }
    }
}
