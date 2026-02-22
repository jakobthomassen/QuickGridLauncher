using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace QuickGridLauncher.Helpers
{
    public static class IconHelper
    {
        public static ImageSource? GetIcon(string path)
        {
            try
            {
                using var icon = Icon.ExtractAssociatedIcon(path);
                if (icon == null) return null;

                using var bmp = icon.ToBitmap();
                using var memory = new MemoryStream();
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memory;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();

                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}