using System;
using System.Globalization;
using System.Windows.Data;

namespace Testing.Common.Converters
{
    [ValueConversion(typeof(UInt32), typeof(string))]
    public sealed class UInt32ToStringConverter : IValueConverter
    {
        #region IValueConverter implementation
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? 0u.ToString(culture) : ((UInt32) value).ToString(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                UInt32 result;

                if (UInt32.TryParse((string) value, out result))
                    return result;
            }

            return 0u;
        }
        #endregion
    }
}
