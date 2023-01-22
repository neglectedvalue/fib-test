using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Configuration;

public static class ServiceCollectionExtensions
{
    public static TSetting ConfigureSettings<TSetting>(this IServiceCollection services,
        IConfiguration configuration, string? configurationSectionName = null)
        where TSetting : class, new()
    {
        var sectionName = configurationSectionName ?? typeof(TSetting).Name;
        var section = configuration.GetSectionSettings<TSetting>(sectionName);

        services.Configure<TSetting>(section!);

        var settingInstance = section.GetSettingInstance<TSetting>(sectionName);

        return settingInstance;
    }

    [UsedImplicitly]
    public static TSetting ConfigureAndValidateSettings<TSetting, TValidator>(this IServiceCollection services, IConfiguration configuration, string? configurationSectionName = null)
            where TSetting : class, new()
            where TValidator : AbstractSettingsValidator<TSetting>, new()
    {
        var sectionName = configurationSectionName ?? typeof(TSetting).Name;
        var settingInstance = services.ConfigureSettings<TSetting>(configuration, sectionName);

        var validatorResult = new TValidator().Validate(sectionName, settingInstance);
        if (validatorResult.Failed)
        {
            throw new OptionsValidationException(sectionName, typeof(TSetting), new[] { validatorResult.FailureMessage });
        }

        services.AddSingleton<IValidateOptions<TSetting>, TValidator>();

        return settingInstance;
    }
}