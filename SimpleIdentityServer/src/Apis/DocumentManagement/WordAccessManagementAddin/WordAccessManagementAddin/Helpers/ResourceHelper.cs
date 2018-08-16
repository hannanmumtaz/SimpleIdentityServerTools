using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace WordAccessManagementAddin.Helpers
{
    internal static class ResourceHelper
    {
        public static Image GetImage(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assm = Assembly.GetExecutingAssembly();
            using (var stream = assm.GetManifestResourceStream(resourceName))
            {
                return Image.FromStream(stream);
            }
        }

        public static BitmapImage GetBitmapImage(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assm = Assembly.GetExecutingAssembly();
            var image = new BitmapImage();
            using (var stream = assm.GetManifestResourceStream(resourceName))
            {
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }
    }
}
