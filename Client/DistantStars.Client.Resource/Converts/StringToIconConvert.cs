using System;
using System.Globalization;
using System.Windows.Data;

namespace DistantStars.Client.Resource.Converts
{
    public class StringToIconConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;
            if (value is string v)
            {
                result = (char)int.Parse(v, NumberStyles.HexNumber);
            }
            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
