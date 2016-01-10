namespace Eugenie.Clients.Common.Validations
{
    using System.Globalization;
    using System.Windows.Controls;

    using Eugenie.Common.Constants;

    public class NameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var valueAsString = ((string) value).Trim();
            if (valueAsString.Length < ValidationConstants.ProductNameMinLength)
            {
                return new ValidationResult(false, $"Името трябва да бъде поне {ValidationConstants.ProductNameMinLength} символа");
            }

            if (valueAsString.Length > ValidationConstants.ProductNameMaxLength)
            {
                return new ValidationResult(false, $"Името трябва да бъде по - кратко от {ValidationConstants.ProductNameMaxLength} символа");
            }

            return ValidationResult.ValidResult;
        }
    }
}