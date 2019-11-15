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
        public TabHost TabControl { get; private set; }

        public Config Config { get; set; }
        public Prefs Prefs { get; set; }
        public VersionsResponse Versions { get; set; }

        public readonly MainPage MainPage = new MainPage();
        public readonly LocalPage LocalPage = new LocalPage();
        public readonly DownloadsPage DownloadsPage = new DownloadsPage();
        //public readonly SettingsPage SettingsPage = new SettingsPage();

        // private
        readonly DataManager<Config> ConfigManager = new DataManager<Config>(App.GetAbsolutePath(App.ConfigPath));
        readonly DataManager<Prefs> PrefsManager = new DataManager<Prefs>(App.GetAbsolutePath(App.PrefsPath));

        public MainWindow()
        {
            InitializeComponent();

            Title = App.Title;
            Version.Content = $"v {App.Version}";

            MainPage.MainWindow = this;
            LocalPage.MainWindow = this;
            DownloadsPage.MainWindow = this;

            try
            {
                Config = ConfigManager.Load();
                Prefs = PrefsManager.Load();
            }
            catch (Exception e)
            {
                ThrowException(e);
                Close();
            }

            if (Config != null && Prefs != null)
            {
                Image.Load(Config.Logo);
                BackgroundImage.Load(Config.Background);
                Title = Config.Title ?? "";
                Width = Prefs.Width;
                Height = Prefs.Height;
                WindowState = Prefs.WindowState;

                TabControl = new TabHost(Tabs, FrameView, Tab, new Page[] { MainPage, LocalPage, DownloadsPage /*, SettingsPage*/ });

                GetVersionsFromWeb((response) =>
                {
                    Versions = response;
                    DownloadsPage.Update(response);
                });

                Closing += (s, e) => {
                    Prefs.Width = Width;
                    Prefs.Height = Height;
                    Prefs.WindowState = WindowState;
                    PrefsManager.Save(Prefs, (ee) => ThrowException(ee));
                };
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
            webClient.Get(Config.VersionsURL ?? "http://google.com", (message) =>
            {
                try
                {
                    WebClient.ProcessResponse(message,
                        (data) =>
                        {
                            var result = JsonUtility.FromJson<VersionsResponse>(data);
                            onDone?.Invoke(result);
                        }, (code) =>
                        {
                            Alert("Error: " + code);
                        });
                }
                catch (Exception ex) 
                {
                    ThrowException(ex);
                }
            }, ThrowException);
        }

        /*public T CloneControl<T>(T obj) where T : FrameworkElement
        {
            return (T)XamlReader.Parse(XamlWriter.Save(obj));
        }*/

        // Alert windows
        public void Alert(string text, string title = "Alert") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        public void Warning(string text, string title = "Warning") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        public void Error(string text, string title = "Error") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ThrowException(Exception exception) => Error(exception.ToString(), exception.GetType().Name);
    }
}
