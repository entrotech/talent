using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Talent.WpfClient.ValidationRules
{
    /// <summary>
    /// Validates that a DateTime  is before today.
    /// </summary>
    public class DateTimePastRule : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            DateTime dt;
            if (value != null
                && value.ToString().Length > 0
                && DateTime.TryParse(value.ToString(), out dt))
            {
                if (dt > DateTime.Today)
                {
                    return new ValidationResult(false, "Date must be in the past");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
