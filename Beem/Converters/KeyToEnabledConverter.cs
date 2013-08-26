using System;
using System.Windows.Data;

namespace Beem.Converters
{
    public class KeyToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (!string.IsNullOrWhiteSpace(value.ToString()))
                    if (parameter != null && parameter.ToString() == "reverse")
                        return false;
                    else
                        return true;
                else
                    if (parameter != null && parameter.ToString() == "reverse")
                        return true;
                    else
                        return false;
            }
            else
            {
                if (parameter != null && parameter.ToString() == "reverse")
                    return true;
                else
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
