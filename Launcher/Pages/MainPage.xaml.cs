using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher.Views;

namespace Launcher.Pages
{
    public partial class MainPage : Page, ITabPage
    {
        public MainWindow MainWindow { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        public void OnShown()
        {
            VersionLabel.Content = "";

            if (MainWindow?.Prefs != null)
            {
                VersionLabel.Content = MainWindow.Prefs.DefaultVersion ?? "";
            }
        }

        public void OnHidden()
        {
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow?.Prefs != null)
            {
                if (string.IsNullOrEmpty(MainWindow.Prefs.DefaultVersionPath))
                {
                    MainWindow.TabControl.Navigate(MainWindow.LocalPage);
                }
                else
                {
                    try
                    {
                        Process.Start(MainWindow.Prefs.DefaultVersionPath);
                        MainWindow.Close();
                    }
                    catch (Exception ex)
                    {
                        MainWindow.Error($"Ошибка запуска ({ex.GetType().Name})", "Error");
                        MainWindow.TabControl.Navigate(MainWindow.LocalPage);
                    }
                }
            }
        }
    }
}
