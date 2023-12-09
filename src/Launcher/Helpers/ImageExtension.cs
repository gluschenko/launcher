using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Launcher.Helpers
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
