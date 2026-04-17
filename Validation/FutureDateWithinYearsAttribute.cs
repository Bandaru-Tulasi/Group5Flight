using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group5Flight.Validation
{
    public class FutureDateWithinYearsAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _maxYears;

        public FutureDateWithinYearsAttribute(int maxYears)
        {
            _maxYears = maxYears;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var date = (DateTime)value;
            var today = DateTime.Today;
            var maxDate = today.AddYears(_maxYears);

            if (date <= today)
            {
                return new ValidationResult(ErrorMessage ?? "Date must be in the future.");
            }

            if (date > maxDate)
            {
                return new ValidationResult(ErrorMessage ?? $"Date cannot be more than {_maxYears} years in the future.");
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-futuredatewithinyears", ErrorMessage ?? $"Date must be between tomorrow and {_maxYears} years from today.");
            MergeAttribute(context.Attributes, "data-val-futuredatewithinyears-maxyears", _maxYears.ToString());
        }

        private bool MergeAttribute(System.Collections.Generic.IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}