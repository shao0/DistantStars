using System;
using System.Globalization;
using System.Windows.Data;

namespace DistantStars.Client.BarrageModule.Converts
{
    public class ValueToBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = parameter != null && value != null && value.ToString().Equals(parameter.ToString());
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value != null && (bool)value ? parameter : null;
            return result;
        }
    }
}
