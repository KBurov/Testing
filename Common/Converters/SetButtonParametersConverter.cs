using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Testing.Common.Converters
{
    public class SetButtonParametersConverter : IMultiValueConverter
    {
        #region IMultiValueConverter implementation
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values != null
                       ? Tuple.Create(values[0] as Page, values[1])
                       : Tuple.Create((Page) null, (object) null);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
