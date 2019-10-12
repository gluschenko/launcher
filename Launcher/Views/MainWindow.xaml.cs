using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Markup;
using Launcher.Core;
using Launcher.Pages;

namespace Launcher.Views
{
    public partial class MainWindow : Window
    {
        readonly DataManager<Config> ConfigManager = new DataManager<Config>("Launcher.json");
        readonly Config Config;

        readonly MainPage MainPage = new MainPage();
        readonly LocalPage LocalPage = new LocalPage();
        readonly DownloadsPage DownloadsPage = new DownloadsPage();
        readonly SettingsPage SettingsPage = new SettingsPage();

        public MainWindow()
        {
            InitializeComponent();

            Config = ConfigManager.Load(ThrowException);

            Title = Config.Title ?? App.Title;
            Version.Content = $"v {App.Version}";

            if (!string.IsNullOrEmpty(Config.Logo))
            {
                try
                {
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.Logo);
                    ImageSource imageSource = new BitmapImage(new Uri(path));
                    Image.Source = imageSource;
                }
                catch (Exception e) {
                    ThrowException(e);
                }
            }

            FrameView.Navigate(MainPage);

            FillTabBar(Tabs, CloneControl<Button>(Tab), MainPage, LocalPage, DownloadsPage, SettingsPage);
        }

        public void FillTabBar(StackPanel panel, Button tab, params Page[] pages)
        {
            panel.Children.Clear();
            foreach (var page in pages)
            {
                var tabCopy = CloneControl<Button>(tab);
                tabCopy.Content = page.Title;
                tabCopy.Click += (s, e) => FrameView.Navigate(page);
                panel.Children.Add(tabCopy);
            }
        }

        public T CloneControl<T>(T obj) where T : FrameworkElement
        {
            /*var newObj = Activator.CreateInstance<T>();
            newObj.DataContext = obj.DataContext;
            return newObj;*/

            return (T)XamlReader.Parse(XamlWriter.Save(obj));
        }

        // Alert windows
        public void Alert(string text, string title = "Alert") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        public void Warning(string text, string title = "Warning") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        public void Error(string text, string title = "Error") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ThrowException(Exception exception) => Error(exception.ToString(), exception.GetType().Name);
    }
}
