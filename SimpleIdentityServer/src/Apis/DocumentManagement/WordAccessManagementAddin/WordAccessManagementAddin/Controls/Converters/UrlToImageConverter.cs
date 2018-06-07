using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WordAccessManagementAddin.Helpers;

namespace WordAccessManagementAddin.Controls.Converters
{
    internal sealed class UrlToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }

            Uri uri;
            if (Uri.TryCreate(value.ToString(), UriKind.RelativeOrAbsolute, out uri))
            {
                return new BitmapImage(uri);
            }
            return ResourceHelper.GetBitmapImage(value.ToString());

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
