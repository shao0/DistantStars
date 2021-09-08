using System;
using System.Globalization;
using System.Windows.Data;

namespace DistantStars.Client.Resource.Converts
{
    public class IntToBooleanConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Equals(parameter?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && b ? parameter : null;
        }
    }
}
