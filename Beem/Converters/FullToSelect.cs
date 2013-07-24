using System;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Linq;
using Beem.Core.Models;

namespace Beem.Converters
{
    public class FullToSelect : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var set = (ObservableCollection<Track>)value;
            if (set != null)
                return set.Take(4);
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
