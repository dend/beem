using System;
using System.Windows.Data;

namespace Beem.Converters
{
    public class KeyToButtonTypeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string key = value.ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    return "Deauthorize";
                }
                else
                {
                    return "Authorize";
                }
            }
            else
                return "Authorize";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
