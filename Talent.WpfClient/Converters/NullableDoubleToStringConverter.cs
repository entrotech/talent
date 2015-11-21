using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Talent.WpfClient.Converters
{
    public class NullableIntToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? "" : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return (int?)null;
            int val  ;
            if (Int32.TryParse(value.ToString(), out val))
            {
                return val;
            }
            return null;
        }

        #endregion
    }
}
