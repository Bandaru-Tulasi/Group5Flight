using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group5Flight.Models.Validation
{
    public class PriceRangeAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly decimal _minValue;
        private readonly decimal _maxValue;

        public PriceRangeAttribute(double minValue, double maxValue)
        {
            _minValue = (decimal)minValue;
            _maxValue = (decimal)maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var price = (decimal)value;

            if (price < _minValue)
            {
                return new ValidationResult(ErrorMessage ?? $"Price cannot be less than ${_minValue}.");
            }

            if (price > _maxValue)
            {
                return new ValidationResult(ErrorMessage ?? $"Price cannot exceed ${_maxValue}.");
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
            MergeAttribute(context.Attributes, "data-val-pricerange", ErrorMessage ?? $"Price must be between ${_minValue} and ${_maxValue}.");
            MergeAttribute(context.Attributes, "data-val-pricerange-minvalue", _minValue.ToString());
            MergeAttribute(context.Attributes, "data-val-pricerange-maxvalue", _maxValue.ToString());
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