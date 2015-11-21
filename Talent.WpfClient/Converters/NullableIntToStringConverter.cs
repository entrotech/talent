using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Talent.WpfClient.Converters
{
    public class NullableDoubleToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? "" : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return (double?)null;
            double dbl;
            if (Double.TryParse(value.ToString(), out dbl))
            {
                return dbl;
            }
            return null;
        }

        #endregion
    }
}
