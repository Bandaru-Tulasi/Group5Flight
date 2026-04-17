using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group5Flight.Validation
{
    public class MaxEmissionAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _maxValue;

        public MaxEmissionAttribute(int maxValue)
        {
            _maxValue = maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var emissionString = value.ToString();
            if (int.TryParse(emissionString, out int emissionValue))
            {
                if (emissionValue > _maxValue)
                {
                    return new ValidationResult(ErrorMessage ?? $"Emission cannot exceed {_maxValue} kg CO2e.");
                }
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
            MergeAttribute(context.Attributes, "data-val-maxemission", ErrorMessage ?? $"Emission cannot exceed {_maxValue} kg CO2e.");
            MergeAttribute(context.Attributes, "data-val-maxemission-maxvalue", _maxValue.ToString());
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