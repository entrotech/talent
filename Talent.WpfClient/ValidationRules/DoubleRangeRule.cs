using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Talent.WpfClient.ValidationRules
{
    /// <summary>
    /// Validates that a string input, if convertible to an double, falls between
    /// a minimum and maximum value
    /// </summary>
    public class DoubleRangeRule : ValidationRule
    {
        public double MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        private double _minValue = double.MinValue;

        public double MaxValue
        {
            get { return _maxvalue; }
            set { _maxvalue = value; }
        }
        private double _maxvalue = double.MaxValue;

        public override ValidationResult Validate(
            object value, System.Globalization.CultureInfo cultureInfo)
        {
            double d;
            // This validation rule is not responsible for 
            // validating that the value is not null or 
            // a double, just that it is in the range.
            if (value == null
                || value.ToString().Length == 0
                || !Double.TryParse(value.ToString(), out d))
                return new ValidationResult(true, null);
            if (d < MinValue)
                return new ValidationResult(false,
                    "Value must be no less than " + MinValue);
            if (d > MaxValue)
                return new ValidationResult(false,
                    "Value must be no greater than " + MaxValue);
            return new ValidationResult(true, null);
        }
    }
}
