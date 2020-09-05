#region Usings

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace OneHundredAndEightyCore.Resources
{
    public class MultipleBoolInvertedAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
                              object parameter, CultureInfo culture)
        {
            bool result1;
            bool result2;
            bool result3;
            switch (values.Length)
            {
                case 1:
                    bool.TryParse(values[0].ToString(), out result1);
                    return !result1;
                case 2:
                    bool.TryParse(values[0].ToString(), out result1);
                    bool.TryParse(values[1].ToString(), out result2);
                    return !result1 && !result2;
                case 3:
                    bool.TryParse(values[0].ToString(), out result1);
                    bool.TryParse(values[1].ToString(), out result2);
                    bool.TryParse(values[2].ToString(), out result3);
                    return !result1 &&
                           !result2 &&
                           !result3;
                default:
                    return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
                                    object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}