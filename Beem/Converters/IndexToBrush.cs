using System;
using System.Windows.Media;
using System.Windows.Data;
using Beem.Core.Models;

namespace Beem.Converters
{
    public class IndexToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int ival = Binder.Instance.CurrentStation.TrackList.IndexOf((Track)value);

            if (ival % 2 == 0)
            {
                return new SolidColorBrush(Color.FromArgb(255, 148, 148, 150));
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 170, 170, 170));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
