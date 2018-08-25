namespace WCR.Common.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NotEqualToAttribute : ValidationAttribute
    {
        private readonly string otherPropertyName;

        public NotEqualToAttribute(string otherPropertyName)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (int)value;
            var otherProperty = validationContext.ObjectType.GetProperty(otherPropertyName);
            if (otherProperty == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (int)otherProperty.GetValue(validationContext.ObjectInstance);
            if (currentValue == comparisonValue)
                return new ValidationResult(this.ErrorMessageString);

            return ValidationResult.Success;
        }
    }
}
