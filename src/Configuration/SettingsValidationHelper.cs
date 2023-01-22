using Microsoft.Extensions.Options;

namespace Configuration;

public static class SettingsValidationHelper
{
    public static void Validate<TSetting, TValidator>(TSetting settings, string? settingsSectionName = null)
        where TSetting : class
        where TValidator : AbstractSettingsValidator<TSetting>, new()
    {
        var sectionName = settingsSectionName ?? typeof(TSetting).Name;
        var validatorResult = new TValidator().Validate(sectionName, settings);

        if (validatorResult.Failed)
        {
            throw new OptionsValidationException(sectionName, typeof(TSetting), new[] { validatorResult.FailureMessage });
        }
    }
}