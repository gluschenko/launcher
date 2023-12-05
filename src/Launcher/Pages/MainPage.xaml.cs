using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Launcher.Views;

namespace Launcher.Pages
{
    public partial class MainPage : Page, ITabPage
    {
        //private readonly MainWindow _mainWindow;
        private readonly LocalPage _localPage;

        public MainPage(/*MainWindow mainWindow,*/ LocalPage localPage)
        {
            InitializeComponent();
            //_mainWindow = mainWindow;
            _localPage = localPage;
        }

        public void OnShown()
        {
            VersionLabel.Content = "";

            /*if (_mainWindow?.Prefs != null)
            {
                VersionLabel.Content = _mainWindow.Prefs.DefaultVersion ?? "";
            }*/
        }

        public void OnHidden()
        {
        }

        private void PlayButton_Click(object? sender, RoutedEventArgs e)
        {
            /*if (_mainWindow.Prefs != null)
            {
                if (string.IsNullOrEmpty(_mainWindow.Prefs.DefaultVersionPath))
                {
                    _mainWindow.TabControl.Navigate(_localPage);
                }
                else
                {
                    try
                    {
                        Process.Start(_mainWindow.Prefs.DefaultVersionPath);
                        _mainWindow.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
                        _mainWindow.TabControl.Navigate(_localPage);
                    }
                }
            }*/
        }


    }
}
