using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Testing.Common.Converters
{
    public sealed class ImageCacheConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var path = (string) value;
            // load the image, specify CacheOption so the file is not locked
            var image = new BitmapImage();
            if (!string.IsNullOrEmpty(path))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(path);
                image.EndInit();
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Not implemented.");
        }
    }
}