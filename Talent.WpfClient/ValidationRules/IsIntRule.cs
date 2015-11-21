using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Talent.WpfClient.ValidationRules
{
    /// <summary>
    /// Validates that a string is convertible to an int.
    /// </summary>
    public class IsIntRule : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i;
            if (value != null
                && value.ToString().Length > 0
                && !Int32.TryParse(value.ToString(), out i))
                return new ValidationResult(false, "Value must an integer.");
            return ValidationResult.ValidResult;
        }
    }
}
