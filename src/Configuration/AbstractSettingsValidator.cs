using Microsoft.Extensions.Options;
using FluentValidation;

namespace Configuration;

public abstract class AbstractSettingsValidator<T> : AbstractValidator<T>, IValidateOptions<T>
    where T : class
{
    public ValidateOptionsResult Validate(string? name, T options)
    {
        var validationResult = Validate(options);

        if (validationResult.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        return ValidateOptionsResult.Fail(
            $"Configuration for '{typeof(T)}' is not valid. Errors: {validationResult}");
    }
}