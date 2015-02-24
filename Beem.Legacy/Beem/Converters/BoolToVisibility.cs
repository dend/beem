using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace Beem.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool currentValue = (bool)value;
            if (currentValue)
            {
                if (parameter.ToString() == "normal")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
            {
                if (parameter.ToString() == "normal")
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.WriteLine("BoolToVisibility Converter: NOT IMPLEMENTED");
            return null;
        }
    }
}
