using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Talent.WpfClient.ValidationRules
{
    /// <summary>
    /// Validates that a string input, if convertible to an int, falls between
    /// a minimum and maximum value
    /// </summary>
    public class IntRangeRule : ValidationRule
    {
        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        private int _minValue = int.MinValue;

        public int MaxValue
        {
            get { return _maxvalue; }
            set { _maxvalue = value; }
        }
        private int _maxvalue = int.MaxValue;

        public override ValidationResult Validate(
            object value, System.Globalization.CultureInfo cultureInfo)
        {
            int d;
            // This validation rule is not responsible for 
            // validating that the value is not null or 
            // a double, just that it is in the range.
            if (value == null
                || value.ToString().Length == 0
                || !Int32.TryParse(value.ToString(), out d))
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
