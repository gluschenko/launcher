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
        // public
        public Config Config { get; set; }
        public VersionsResponse Versions { get; set; }

        public readonly MainPage MainPage = new MainPage();
        public readonly LocalPage LocalPage = new LocalPage();
        public readonly DownloadsPage DownloadsPage = new DownloadsPage();
        public readonly SettingsPage SettingsPage = new SettingsPage();

        // private
        readonly DataManager<Config> ConfigManager = new DataManager<Config>(App.ConfigPath);
        readonly TabHost tabHost;

        public MainWindow()
        {
            InitializeComponent();

            Title = App.Title;
            Version.Content = $"v {App.Version}";

            LocalPage.MainWindow = this;

            try
            {
                Config = ConfigManager.Load();
            }
            catch (Exception e)
            {
                ThrowException(e);
                Close();
            }

            if (Config != null)
            {
                Image.Load(Config.Logo);
                BackgroundImage.Load(Config.Background);
                Title = Config.Title;

                //FillTabBar(Tabs, CloneControl(Tab), MainPage, LocalPage, DownloadsPage, SettingsPage);
                tabHost = new TabHost(Tabs, FrameView, Tab, new Page[] { MainPage, LocalPage, DownloadsPage, SettingsPage });

                GetVersionsFromWeb((response) =>
                {
                    Versions = response;

                    DownloadsPage.Update(response);

                    //Alert(response.Version);
                    //Alert(string.Join(", ", response.WindowsVersions));
                });
            }
            else
            {
                Error("Config is empty: " + ConfigManager.Path);
                Close();
            }
        }

        public void GetVersionsFromWeb(Action<VersionsResponse> onDone)
        {
            IWebClient webClient = new WebClientAsync();
            webClient.Get(Config.VersionsURL, (message) => {
                WebClient.ProcessResponse(message,
                    (data) =>
                    {
                        var result = JsonUtility.FromJson<VersionsResponse>(data);
                        onDone?.Invoke(result);
                    }, (code) =>
                    {
                        Alert("Error: " + code);
                    });
            }, ThrowException);
        }

        public T CloneControl<T>(T obj) where T : FrameworkElement
        {
            return (T)XamlReader.Parse(XamlWriter.Save(obj));
        }

        // Alert windows
        public void Alert(string text, string title = "Alert") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        public void Warning(string text, string title = "Warning") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        public void Error(string text, string title = "Error") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ThrowException(Exception exception) => Error(exception.ToString(), exception.GetType().Name);
    }
}
