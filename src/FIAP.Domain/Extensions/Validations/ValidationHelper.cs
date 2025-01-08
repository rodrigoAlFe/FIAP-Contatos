using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Extensions.Validations;

/// <summary>
/// ValidationHelper provides methods to validate objects using data annotations.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates an object of type <typeparamref name="T"/> using recursive data annotation validation.
    /// </summary>
    /// <typeparam name="T">The type of the object to be validated.</typeparam>
    /// <param name="obj">The object to validate.</param>
    /// <returns>A tuple where the first element is a boolean indicating whether the object is valid,
    /// and the second element is a list of <see cref="ValidationResult"/> containing the validation errors.</returns>
    public static (bool isValid, List<ValidationResult> errors) ValidateObject<T>(T obj)
    {
        var errors = new List<ValidationResult>();
        var isValid = TryValidateObjectRecursive(obj, new ValidationContext(obj!), errors, true);
        return (isValid, errors);
    }

    /// <summary>
    /// Recursively validates an object of type <typeparamref name="T"/> using data annotation validation.
    /// </summary>
    /// <typeparam name="T">The type of the object to be validated.</typeparam>
    /// <param name="obj">The object to validate.</param>
    /// <param name="validationContext">The context for validation.</param>
    /// <param name="results">A list to hold validation errors, if any.</param>
    /// <param name="validateAllProperties">Whether to validate all properties. Pass true to validate all properties; otherwise, false.</param>
    /// <returns>True if the object is valid; otherwise, false.</returns>
    private static bool TryValidateObjectRecursive<T>(T obj, ValidationContext validationContext,
        List<ValidationResult> results, bool validateAllProperties)
    {
        var result = Validator.TryValidateObject(obj!, validationContext, results, validateAllProperties);

        var properties = 
            obj!.GetType()
                .GetProperties()
                .Where(prop => prop.CanRead && prop.GetIndexParameters().Length == 0)
                .ToList();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType) continue;

            var value = obj.GetType().GetProperty(property.Name)?.GetValue(obj, null);

            if (value == null) continue;

            var nestedResults = new List<ValidationResult>();
            var nestedContext = new ValidationContext(value, null, null);
            var nestedValid = TryValidateObjectRecursive(value, nestedContext, nestedResults, validateAllProperties);

            if (nestedValid) continue;
            result = false;
            results.AddRange(
                from validationResult in nestedResults
                let propertyInfo = property
                let key = propertyInfo.Name + '.' + validationResult.MemberNames.FirstOrDefault()
                select new ValidationResult(validationResult.ErrorMessage, [key]));
        }

        return result;
    }
}