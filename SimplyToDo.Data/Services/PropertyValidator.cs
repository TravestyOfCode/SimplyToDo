using System.ComponentModel.DataAnnotations;

namespace SimplyToDo.Data.Services;

internal class PropertyValidator
{
    public IEnumerable<ValidationResult> ValidateModel<T>(T model) where T : notnull
    {
        ValidationContext context = new ValidationContext(model);

        var errors = new List<ValidationResult>();

        _ = Validator.TryValidateObject(model, context, errors, true);

        return errors;
    }
}
