#region Usings

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace OneHundredAndEightyCore.Resources
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = (bool) value;
            return !booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = (bool) value;
            return !booleanValue;
        }
    }
}