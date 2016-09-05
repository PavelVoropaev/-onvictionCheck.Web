using System.ComponentModel.DataAnnotations;

namespace СonvictionCheck.Web.Attributes
{
    public class RequiredIf : ValidationAttribute
    {
        public RequiredIf(string propertyNames)
        {
            PropertyNames = propertyNames;
        }

        public string PropertyNames { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(PropertyNames);
            var values = property.GetValue(validationContext.ObjectInstance, null) as bool?;
            if (values == true && string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}
