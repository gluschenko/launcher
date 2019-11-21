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
using System.ComponentModel;
//using System.Windows.Shapes;
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
                if (!File.Exists(ConfigManager.Path)) 
                {
                    MessageHelper.Error($"File '{Path.GetFileName(ConfigManager.Path)}' does not exist!");
                    Close();
                    return;
                }

                Config = ConfigManager.Load();
                Prefs = PrefsManager.Load();
            }
            catch (Exception e)
            {
                MessageHelper.ThrowException(e);
                Close();
                return;
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

                Closing += OnWindowClosing;
            }
            else
            {
                MessageHelper.Error("Config is empty: " + ConfigManager.Path);
                Close();
                return;
            }
        }

        private void OnWindowClosing(object sender, CancelEventArgs args) 
        {
            Prefs.Width = Width;
            Prefs.Height = Height;
            Prefs.WindowState = WindowState;
            PrefsManager.Save(Prefs, (ee) => MessageHelper.ThrowException(ee));
        }

        public void GetVersionsFromWeb(Action<VersionsResponse> onDone)
        {
            IWebClient webClient = new WebClientAsync();
            webClient.Get(Config.VersionsURL, (message) =>
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
                            MessageHelper.Alert("Error: " + code);
                        });
                }
                catch (Exception ex) 
                {
                    MessageHelper.ThrowException(ex);
                }
            }, MessageHelper.ThrowException);
        }
        
    }
}
