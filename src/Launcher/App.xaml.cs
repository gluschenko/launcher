using System.IO;
using System.Windows;
using Launcher.Core;
using Launcher.Models;
using Launcher.Pages;
using Launcher.Services;
using Launcher.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Launcher
{
    public partial class App : Application
    {
        public const string Title = "Launcher";
        public const string Version = "2023.1";
        public const string ConfigPath = "Launcher.json";
        public const string PrefsPath = "LauncherPrefs.json";
        public const string DownloadsDirectory = "Downloads";
        public const string VersionsDirectory = "Versions";

        private readonly IHost _host;

        public App()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainWindow>();

                    services.AddScoped<MainPage>();
                    services.AddScoped<LocalPage>();
                    services.AddScoped<DownloadsPage>();
                    services.AddScoped<SettingsPage>();

                    services.AddScoped<DataManager<Config>>(x =>
                    {
                        return new(GetAbsolutePath(ConfigPath));
                    });

                    services.AddScoped<DataManager<Prefs>>(x =>
                    {
                        return new(GetAbsolutePath(PrefsPath));
                    });

                    services.AddSingleton<StateManager>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                await _host.StartAsync();

                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }

        public static string GetAbsolutePath(string directory)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);
        }
    }
}
