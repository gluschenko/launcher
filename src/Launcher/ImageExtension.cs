using System;
using System.Collections.Generic;
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
using System.Windows.Markup;
using Launcher.Core;
using Launcher.Pages;

namespace Launcher
{
    public static class ImageExtension
    {
        public static void Load(this Image image, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                    ImageSource imageSource = new BitmapImage(new Uri(path));
                    image.Source = imageSource;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), $"Failed to load: {path}", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
